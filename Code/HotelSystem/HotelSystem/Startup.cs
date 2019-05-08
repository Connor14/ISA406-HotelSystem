using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HotelSystem.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelSystem
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();

            app.Use(async (context, next) =>
            {
                bool isIndex = context.Request.Path.Value.StartsWith("/") || context.Request.Path.Value.StartsWith("/Index");
                bool isLoginRegister = context.Request.Path.Value.StartsWith("/LoginRegister/");
                //bool isLoginPage = context.Request.Path == "/LoginRegister/Login";
                //bool isRegisterPage = context.Request.Path == "/LoginRegister/Register";

                // don't allow access to these unless current hotelId is set
                bool isHotelSpecific = context.Request.Path.Value.StartsWith("/Hotel/") || context.Request.Path.Value.StartsWith("/Loyalty/");
                //bool isAllHotelsPage = context.Request.Path == "/AllHotels";

                string email = context.Session.GetString(Constants.UserEmail);
                int? hotelId = context.Session.GetInt32(Constants.CurrentHotel);

                //Debug.WriteLine(context.Request.Path + " " + emailSet + " " + email);
                if (!isLoginRegister && !isIndex && email == null) // not logged in
                {
                    context.Response.Redirect("/LoginRegister/Login");
                }
                else if (isHotelSpecific && email == null) // is hotel specific, no hotel selected
                {
                    context.Response.Redirect("/LoginRegister/Login");
                }
                else if (isHotelSpecific && hotelId == null) // is hotel specific, no hotel selected
                {
                    context.Response.Redirect("/Index");
                }
                else
                {
                    await next.Invoke();
                }
            });

            app.UseMvc();
        }
    }
}
