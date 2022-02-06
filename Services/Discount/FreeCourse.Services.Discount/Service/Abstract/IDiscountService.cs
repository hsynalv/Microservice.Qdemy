using FreeCourse.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discount_ = FreeCourse.Services.Discount.Models.Discount;

namespace FreeCourse.Services.Discount.Service.Abstract
{
    public interface IDiscountService
    {
        Task<Response<List<Discount_>>> GetAll();
        Task<Response<Discount_>> GetById(int id);
        Task<Response<NoContent>> Save(Discount_ discount);
        Task<Response<NoContent>> DeleteById(int id);
        Task<Response<NoContent>> Update(Discount_ discount);
        Task<Response<Discount_>> GetByCodeAndUserId(string code, string userId);   
    }
}
