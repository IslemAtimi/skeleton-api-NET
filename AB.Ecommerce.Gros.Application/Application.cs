using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace AB.Ecommerce.Gros.Application
{
    public class Application
    {
        public Application()
        {

        }

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Application));

            services.AddTransient<IClientAppService, ClientAppService>();
            services.AddTransient<ICategoryAppService, CategoryAppService>();


        }
    }

}

