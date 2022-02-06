using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Services.Basket.DTOs
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice
        {
            get => BasketItems.Sum(I => I.Price * I.Quantity);
        }

    }
}
