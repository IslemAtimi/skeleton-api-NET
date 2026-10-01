using AB.Ecommerce.Gros.Business;
using AB.Ecommerce.Gros.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AB.Ecommerce.Gros.Application
{
    public class Data
    {
        public Data()
        {

        }

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();




        }
    }

}

