using AutoMapper;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

namespace Notary.Api.DataAccess.Models.DTOs
{
    public  class MapperConfig:Profile
    {
        public MapperConfig()
        {  
            CreateMap<PageDTO, Page>();          
            CreateMap<ComponentDTO, Notary.CMS.DataAccess.Models.Component>();
            CreateMap<ApplicationDTO, Application>();

            CreateMap<Page, PageSDTO>();
            CreateMap<Notary.CMS.DataAccess.Models.Component, ComponentSDTO>();
            CreateMap<Application, ApplicationSDTO>();
            
        }
    }
}
