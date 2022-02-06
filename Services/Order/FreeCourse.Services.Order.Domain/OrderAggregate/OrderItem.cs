using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class OrderItem : Entity
    {
        public string CourseId { get;private set; }
        public string CourseName { get;private set; }
        public string PictureUrl { get;private set; }
        public Decimal Price { get;private set; }

        public OrderItem()
        {

        }
        public OrderItem(string courseId, string courseName, string pictureUrl, decimal price)
        {
            CourseId = courseId;
            CourseName = courseName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        public void UpdateOrderItem(string courseName, string pictureUrl, decimal price)
        {
            CourseName = courseName;
            PictureUrl = pictureUrl;
            Price = price;
        }
    }
}
