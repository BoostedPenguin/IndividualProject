using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using net_core_backend.Profiles;

namespace net_core_backend
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
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                            .AllowAnyMethod()
                                                             .AllowAnyHeader()));
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
            GoogleDataService.GoogleKey = Configuration.GetValue<string>("AppIdentitySettings:GoogleApiKey");


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-3sn-ksqj.eu.auth0.com/";
                options.Audience = "https://localhost:5001/api";

                // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            services.AddDbContext<IndividualProjectContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLCONNSTR_Database"));
            });

            services.AddSingleton<IContextFactory>(new ContextFactory(Configuration.GetConnectionString("SQLCONNSTR_Database")));

            services.AddHttpContextAccessor();

            services.AddSingleton<IAccountService, AccountDataService>();

            services.AddSingleton<ITicketService, TicketDataService>();

            services.AddSingleton<ITripService, TripDataService>();

            services.AddSingleton<IWishListService, WishListDataService>();

            services.AddSingleton<IGoogleService, GoogleDataService>();

            services.AddHttpClient();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
