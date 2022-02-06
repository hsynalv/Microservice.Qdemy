using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.PhotoStocks;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0)
                return null;

            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            using var ms = new MemoryStream();

            await  photo.CopyToAsync(ms);

            var multiPartContent = new MultipartFormDataContent();
            multiPartContent.Add(new ByteArrayContent(ms.ToArray()),"photo",randomFileName);

            var response = await _httpClient.PostAsync("photos", multiPartContent);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return result.Data;
        }
    }
}
