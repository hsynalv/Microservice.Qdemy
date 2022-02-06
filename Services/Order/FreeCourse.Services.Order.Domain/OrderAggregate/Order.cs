using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public  class Order : Entity, IAgggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Adress { get;private set; }
        public string UserId { get;private set; }

        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);

        public Order()
        {

        }
        public Order(Address adress, string userId)
        {
            _orderItems = new();
            CreatedDate = DateTime.Now;
            UserId = userId;
            Adress = adress;
        }

        public void AddOrderItem(string courseId, string courseName, decimal price, string pictureUrl)
        {
            var existCourse = _orderItems.Any(x => x.CourseId == courseId);

            if (!existCourse)
                _orderItems.Add(new(courseId, courseName, pictureUrl, price));
        }

        

        
    }
}
