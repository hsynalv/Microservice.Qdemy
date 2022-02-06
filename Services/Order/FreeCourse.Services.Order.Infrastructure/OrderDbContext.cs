using FreeCourse.Services.Order.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using _Order = FreeCourse.Services.Order.Domain.OrderAggregate.Order;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext>options) : base(options)
        {
        }

        public DbSet<_Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<_Order>().ToTable("Orders",DEFAULT_SCHEMA);
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems",DEFAULT_SCHEMA);

            modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<_Order>().OwnsOne(o => o.Adress).WithOwner();

        }
    }
}
