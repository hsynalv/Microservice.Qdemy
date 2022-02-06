using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Service.Abstract
{
    public interface ICategoryService
    {
         Task<Response<List<CategoryDto>>> GetAllAsync();

         Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);

         Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
