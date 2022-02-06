using FreeCourse.Web.Models.Settings;
using FreeCourse.Web.Services.Abstract;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSetting _serviceApiSetting;
        private readonly ClientSetting _clientSetting;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(IOptions<ServiceApiSetting> serviceApiSetting, IOptions<ClientSetting> clientSetting, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
        {
            _serviceApiSetting = serviceApiSetting.Value;
            _clientSetting = clientSetting.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }

        public async Task<string> GetToken()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken");
            if (currentToken != null)
                return currentToken.AccessToken;

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSetting.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (discovery.IsError)
                throw discovery.Exception;

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSetting.WebClient.ClientId,
                ClientSecret = _clientSetting.WebClient.ClientSecret,
                Address = discovery.TokenEndpoint,
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken.IsError)
                throw newToken.Exception;

            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn);

            return newToken.AccessToken;

        }
    }
}
