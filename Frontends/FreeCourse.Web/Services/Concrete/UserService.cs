using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpCLient;

        public UserService(HttpClient httpCLient)
        {
            _httpCLient = httpCLient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await _httpCLient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
        }
    }
}
