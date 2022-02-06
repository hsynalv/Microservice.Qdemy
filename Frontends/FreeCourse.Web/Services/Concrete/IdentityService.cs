using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Settings;
using FreeCourse.Web.Services.Abstract;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class IdentityService : IIdentityServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSetting _clientSetting;
        private readonly ServiceApiSetting _serviceApiSetting;

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor,
            IOptions<ClientSetting> clientSetting, IOptions<ServiceApiSetting> serviceApiSetting)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSetting = clientSetting.Value;
            _serviceApiSetting = serviceApiSetting.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await DiscoveryEndpoints();

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSetting.WebClientForUser.ClientId,
                ClientSecret = _clientSetting.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                return null;
            }

            var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var discovery = await DiscoveryEndpoints();

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSetting.WebClientForUser.ClientId,
                ClientSecret = _clientSetting.WebClientForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SigninInput signInInput)
        {
            var discovery = await DiscoveryEndpoints();

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSetting.WebClientForUser.ClientId,
                ClientSecret = _clientSetting.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = discovery.TokenEndpoint
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);

        }

        private async Task<DiscoveryDocumentResponse> DiscoveryEndpoints()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSetting.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (discovery.IsError)
                throw discovery.Exception;

            return discovery;
        }
    }
}
