using FreeCourse.Services.FakePayment.Model;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndpoit =await  _sendEndpointProvider.GetSendEndpoint(new Uri("queue:created-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand
            {
                District = paymentDto.Order.Address.District,
                Line = paymentDto.Order.Address.Line,
                Province = paymentDto.Order.Address.Province,
                Street = paymentDto.Order.Address.Street,
                UserId = paymentDto.Order.UserId,
                ZipCode = paymentDto.Order.Address.ZipCode
            };
            paymentDto.Order.OrderItems.ForEach(item =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = item.PictureUrl,Price = item.Price,ProductId = item.ProductId,ProductName = item.ProductName,
                });
            });

            await sendEndpoit.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
        }


    }
}
