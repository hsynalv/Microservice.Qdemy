using FluentValidation.AspNetCore;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using FreeCourse.Web.Extensions;
using FreeCourse.Web.Handler;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Settings;
using FreeCourse.Web.Services.Abstract;
using FreeCourse.Web.Services.Concrete;
using FreeCourse.Web.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FreeCourse.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceApiSetting>(Configuration.GetSection("ServiceApiSettings"));
            services.Configure<ClientSetting>(Configuration.GetSection("ClientSettings"));
            
            
            services.AddHttpContextAccessor();

            services.AddAccessTokenManagement();
            services.AddSingleton<PhotoHelper>();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();


            services.AddHttpClientServices(Configuration);



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt => 
            {
                opt.LoginPath = "/Auth/SignIn";
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                opt.SlidingExpiration = true;
                opt.Cookie.Name = "freecoursecookie";
            });


            services.AddControllersWithViews().AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
