using System;

namespace FreeCourse.Services.Order.Application.DTOs
{
    public class OrderItemDto
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string PictureUrl { get; set; }
        public Decimal Price { get; set; }
    }
}
