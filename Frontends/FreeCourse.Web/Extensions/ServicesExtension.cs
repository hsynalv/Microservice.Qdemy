using FreeCourse.Web.Handler;
using FreeCourse.Web.Models.Settings;
using FreeCourse.Web.Services.Abstract;
using FreeCourse.Web.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FreeCourse.Web.Extensions
{
    public static class ServicesExtension
    {
        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var serviceApiSetting = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSetting>();


            services.AddHttpClient<IIdentityServices, IdentityService>();

            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.GatewayBaseUri}/{serviceApiSetting.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.GatewayBaseUri}/{serviceApiSetting.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSetting.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.GatewayBaseUri}/{serviceApiSetting.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.GatewayBaseUri}/{serviceApiSetting.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.GatewayBaseUri}/{serviceApiSetting.Payment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.GatewayBaseUri}/{serviceApiSetting.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();


            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
        }
    }
}
