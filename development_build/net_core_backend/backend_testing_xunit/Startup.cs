using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Profiles;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.DependencyInjection;

namespace backend_testing_xunit
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            services.AddSingleton<IContextFactory>(new ContextFactoryTesting());

            DatabaseSeeder.Seed(new ContextFactoryTesting());
        }
    }
}
