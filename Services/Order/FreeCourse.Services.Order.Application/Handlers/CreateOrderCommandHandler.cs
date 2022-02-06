using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using _Order = FreeCourse.Services.Order.Domain.OrderAggregate.Order;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Address newAddress = new(request.Address.Province, request.Address.District,
                request.Address.Street, request.Address.ZipCode, request.Address.Line);

            _Order newOrder = new(newAddress, request.UserId);

            request.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.CourseId, x.CourseName, x.Price, x.PictureUrl);
            });

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
        }
    }
}
