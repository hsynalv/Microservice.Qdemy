using FreeCourse.Services.Basket.Service.Abstract;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services.Abstract;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumer
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CourseNameChangedEventConsumer(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var basket = await _basketService.GetBasket(_sharedIdentityService.GetUserId);

            foreach (var item in basket.Data.BasketItems)
            {
                if (item.CourseId==context.Message.CourseId)
                {
                    item.CourseName = context.Message.UpdatedName;
                }
            }

            await _basketService.SaveOrUpdate(basket.Data);
        }
    }
}
