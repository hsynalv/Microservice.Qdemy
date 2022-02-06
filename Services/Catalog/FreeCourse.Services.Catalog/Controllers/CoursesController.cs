using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Service.Abstract;
using FreeCourse.Shared.BaseController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers
{
    public class CoursesController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) 
        {
            var response = await _courseService.GetById(id);

            return CreateActionResultInstance(response);
        }

        [HttpGet("id/{userId}")]
        public async Task<IActionResult> GettAllByUserId(string userId)
        {
            var response = await _courseService.GettAllByUserId(userId);

            return CreateActionResultInstance(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCrateDto courseCrateDto) 
        {
            var response = await _courseService.CreateAsync(courseCrateDto);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDto);

            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}
