using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract
{
    public interface IIdentityServices
    {
        Task<Response<bool>> SignIn(SigninInput signInInput);

        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefreshToken();
    }
}
