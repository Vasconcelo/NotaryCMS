using AutoMapper;
using Notary.Api.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Api.DataAccess.Models.DTOs
{
    public  class MapperConfig:Profile
    {
        public MapperConfig()
        {  
            CreateMap<PageDTO, Page>();          
            CreateMap<ComponentDTO, Component>();
            CreateMap<ApplicationDTO, Application>();

            CreateMap<Page, PageSDTO>();
            CreateMap<Component, ComponentSDTO>();
            CreateMap<Application, ApplicationSDTO>();
            
        }
    }
}
