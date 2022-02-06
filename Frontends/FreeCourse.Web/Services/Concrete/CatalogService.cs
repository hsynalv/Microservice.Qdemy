using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Abstract;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class CatalogService : ICatalogService
    {

        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;
        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhoto = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);
            if (resultPhoto != null)
            {
                courseCreateInput.Picture = resultPhoto.Url;
            }

            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");
            if (!response.IsSuccessStatusCode)
                return null;
            var result = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await _httpClient.GetAsync("courses");
            if (!response.IsSuccessStatusCode)
                return null;
            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            result.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/id/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;
            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            result.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return result.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode)
                return null;
            var result = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            result.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(result.Data.Picture);
            return result.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhoto = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);
            if (resultPhoto != null)
            {
                await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhoto.Url;
            }

            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
