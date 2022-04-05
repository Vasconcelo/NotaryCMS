using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notary.CMS.DataAccess.Interfaces;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

namespace Notary.CMS.Api.DataAccess.Repositories
{
    public  class ApplicationRepository:IApplicationRepository
    {
        private readonly NotaryCMSDBContext _ctx;
        private readonly IMapper _mapper;

        public ApplicationRepository(NotaryCMSDBContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public Application CreateApplication(Application app)
        {
            _ctx.Entry(app).State = EntityState.Added;
            _ctx.Add(app);
            _ctx.SaveChanges();
            return app;
        }

        public Application UpdateApplication(ApplicationDTO model, int id)
        {
            var app = _ctx.Applications.First(p => p.Id == id);

            app.Identifier = model.Identifier;
            app.Name = model.Name;        
            app.UpdatedAt = DateTime.Now;
            _ctx.Entry(app).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _ctx.SaveChanges();
            return app;
        }

        public Application GetApplication(int id)
        {
            Application? app = new Application();
            app = _ctx.Applications.FirstOrDefault(x => x.Id == id);
            return app;
        }


        public void DeleteApplication(Application app)
        {           
            _ctx.Remove(app);   
            _ctx.SaveChanges();            
        }       

        public List<ApplicationSDTO> GetApplications()
        {
            return _mapper.Map<List<ApplicationSDTO>>(_ctx.Applications.ToList());
        }       
    }
}
