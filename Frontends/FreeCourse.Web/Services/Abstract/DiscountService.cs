using FreeCourse.Web.Models.Discount;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}
