using System;

namespace FreeCourse.Web.Models.Orders
{
    public class OrderItemViewModel
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}