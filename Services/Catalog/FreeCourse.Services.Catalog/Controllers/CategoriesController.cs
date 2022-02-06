using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Service.Abstract;
using FreeCourse.Shared.BaseController;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers
{
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            var categories = await _categoryService.GetAllAsync();

            return CreateActionResultInstance(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryDto categoryDto) 
        {
            var response = await _categoryService.CreateAsync(categoryDto);

            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id) 
        {
            var category = await _categoryService.GetByIdAsync(id);

            return CreateActionResultInstance(category);
        }


    }
}
