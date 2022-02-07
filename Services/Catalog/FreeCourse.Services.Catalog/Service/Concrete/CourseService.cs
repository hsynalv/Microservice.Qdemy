using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Service.Abstract;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using MassTransit;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Shared.Messages;

namespace FreeCourse.Services.Catalog.Service.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseColleciton;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, ICategoryService categoryService, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseColleciton = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _categoryService = categoryService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Shared.DTOs.Response<List<CourseDto>>> GetAllAsync() 
        {
            var courses = await _courseColleciton.Find(course => true).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Shared.DTOs.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses),200);

        } 

        public async Task<Shared.DTOs.Response<CourseDto>> GetById(string id) 
        {
            var course = await _courseColleciton.Find<Course>(course => course.Id == id).FirstOrDefaultAsync();

            if (course == null) 
            {
                return Shared.DTOs.Response<CourseDto>.Fail("Course not found", 404);
            }

            var categoryDto = (await _categoryService.GetByIdAsync(course.CategoryId)).Data;
            course.Category = _mapper.Map<Category>(categoryDto);

            return Shared.DTOs.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Shared.DTOs.Response<List<CourseDto>>> GettAllByUserId(string userId) 
        {
            var courses = await _courseColleciton.Find<Course>(I => I.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Shared.DTOs.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Shared.DTOs.Response<CourseDto>> CreateAsync(CourseCrateDto courseCrateDto) 
        {
            var newCourse = _mapper.Map<Course>(courseCrateDto);
            await _courseColleciton.InsertOneAsync(newCourse);

            return Shared.DTOs.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 201);
        }

        public async Task<Shared.DTOs.Response<NoContent>> UpdateAsync(CourseUpdateDto coursUpdateDto) 
        {
            var updatedCourse = _mapper.Map<Course>(coursUpdateDto);
            var result = await _courseColleciton.FindOneAndReplaceAsync(I => I.Id == coursUpdateDto.Id, updatedCourse);

            if (result == null) 
            {
                return Shared.DTOs.Response<NoContent>.Fail("Courses Not Found", 404);
            }


            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent() { CourseId = updatedCourse.Id, UpdatedName= updatedCourse.Name });

            return Shared.DTOs.Response<NoContent>.Success(204);
        }

        public async Task<Shared.DTOs.Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseColleciton.DeleteOneAsync(I => I.Id == id);

            if (result.DeletedCount > 0)
                return Shared.DTOs.Response<NoContent>.Success(204);

            return Shared.DTOs.Response<NoContent>.Fail("Course Not Found", 404);
        }
    }
}
