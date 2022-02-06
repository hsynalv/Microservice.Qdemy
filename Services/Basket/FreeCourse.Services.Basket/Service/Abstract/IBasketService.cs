using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.DTOs;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Service.Abstract
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
        Task<Response<bool>> Delete(string userId);

    }
}
