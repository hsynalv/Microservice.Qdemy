using System;
using System.Collections.Generic;

namespace FreeCourse.Services.Order.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public AddressDto Adress { get; set; }
        public string UserId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
