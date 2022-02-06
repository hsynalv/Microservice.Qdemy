using FreeCourse.Web.Models.Settings;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceApiSetting _serviceApiSetting;

        public PhotoHelper(IOptions<ServiceApiSetting> serviceApiSetting)
        {
            _serviceApiSetting = serviceApiSetting.Value;
        }

        public string GetPhotoStockUrl(string photoUrl)
        {
            return $"{_serviceApiSetting.PhotoStockUri}/photos/{photoUrl}";
        }
    }
}
