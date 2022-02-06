using System;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract
{
    public interface IClientCredentialTokenService
    {
        Task<String> GetToken();
    }
}
