using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Services.Basket.Service.Abstract;
using FreeCourse.Shared.DTOs;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Service.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket Coult Not Fpund", 404);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
                return Response<BasketDto>.Fail("Basket Not Found",404);

            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket),200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket Coult Not Save Or Update", 500);
        }
    }
}
