using System;
using AB.Ecommerce.Gros.Application.Dtos;
using AB.Ecommerce.Gros.Business;
using AutoMapper;

namespace AB.Ecommerce.Gros.Application
{
	public class MappingProfile: Profile
    {
		public MappingProfile()
		{
            CreateMap<Client, ClientDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<ClientFile, FileDto>();
            CreateMap<Client, ClienWithPasswordtDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Category, CategoryDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<CategoryImage, FileDto>();

            
            


        }
    }
}

