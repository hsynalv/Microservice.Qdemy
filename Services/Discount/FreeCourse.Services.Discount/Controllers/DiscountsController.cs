using FreeCourse.Services.Discount.Service.Abstract;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Discount_ = FreeCourse.Services.Discount.Models.Discount;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetById(id);
            return CreateActionResultInstance(discount);
        }

        [HttpGet(("user/{code}"))]
        public async Task<IActionResult> GetByIdAndCode(string code) 
        {
            var discount = await _discountService.GetByCodeAndUserId(code, _sharedIdentityService.GetUserId);
            return CreateActionResultInstance(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Discount_ discount_)
        {
            discount_.UserId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.Save(discount_));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Discount_ discount_)
        {
            return CreateActionResultInstance(await _discountService.Update(discount_));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.DeleteById(id));
        }


    }
}
