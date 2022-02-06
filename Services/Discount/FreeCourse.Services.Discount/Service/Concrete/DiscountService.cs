using Dapper;
using FreeCourse.Services.Discount.Service.Abstract;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Discount_ = FreeCourse.Services.Discount.Models.Discount;

namespace FreeCourse.Services.Discount.Service.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }


        public async Task<Response<NoContent>> DeleteById(int id)
        {
            var check = await GetById(id);
            if (check.StatusCode == 404)
                return Response<NoContent>.Fail(check.Errors, check.StatusCode);

            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });

            if (status > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An error accured while deleted", 500);
        }

        public async Task<Response<List<Discount_>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Discount_>("select * from discount");
            return Response<List<Discount_>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Discount_>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = (await _dbConnection.QueryAsync<Discount_>("select * from discount where userId=@userId and code=@Code",
                new { UserId = userId, Code = code })).FirstOrDefault();

            if (discounts == null)
            {
                return Response<Discount_>.Fail("Discount Not Found", 404);
            }
            return Response<Discount_>.Success(discounts,200);

        }

        public async Task<Response<Discount_>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Discount_>("select * from discount where id=@ID", 
                new { ID = id })).SingleOrDefault();

            if (discount == null)
                return Response<Discount_>.Fail("Discount Not Found", 404);
            return Response<Discount_>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Discount_ discount)
        {
            var status = await _dbConnection.ExecuteAsync(@"INSERT INTO discount (userid,rate,code) 
                                VALUES (@UserId,@Rate,@Code)", discount);

            if (status > 0)
                return Response<NoContent>.Success(204);
            
            return Response<NoContent>.Fail("An error occured while adding",500);
        }

        public async Task<Response<NoContent>> Update(Discount_ discount)
        {
            var check = await GetById(discount.Id);
            if (check.StatusCode == 404)
                return Response<NoContent>.Fail(check.Errors, check.StatusCode);

            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId,code=@Code,rate=@Rate where id=@id", discount);

            if (status > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An error accured while updated", 500);
        }
    }
}
