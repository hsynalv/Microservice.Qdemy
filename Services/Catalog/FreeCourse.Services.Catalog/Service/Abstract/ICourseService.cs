using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Service.Abstract
{
    public interface ICourseService
    {

        Task<Response<List<CourseDto>>> GetAllAsync();

        Task<Response<CourseDto>> GetById(string id);

        Task<Response<List<CourseDto>>> GettAllByUserId(string userId);

        Task<Response<CourseDto>> CreateAsync(CourseCrateDto courseCrateDto);

        Task<Response<NoContent>> UpdateAsync(CourseUpdateDto coursUpdateDto);

        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
