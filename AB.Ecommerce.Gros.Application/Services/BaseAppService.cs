using System;
using System.Security.Claims;
using AB.Ecommerce.Gros.Business;
using AB.Ecommerce.Gros.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AB.Ecommerce.Gros.Application
{
    public class BaseAppService
    {


        protected readonly IMapper Mapper;
        protected readonly ClaimsPrincipal User;


        public BaseAppService()
        {
            Mapper = ServiceLocator.GetService<IMapper>();
            var httpContextAccessor = ServiceLocator.GetService<IHttpContextAccessor>();
            User = httpContextAccessor.HttpContext.User;

        }


    }
    
}

