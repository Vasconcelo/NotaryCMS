using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notary.CMS.DataAccess.Interfaces;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

namespace Notary.Api.DataAccess.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly NotaryCMSDBContext _ctx;
        private readonly IMapper _mapper;

        public ComponentRepository(NotaryCMSDBContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public Component CreateComponent(Component component)
        {
            _ctx.Entry(component).State = EntityState.Added;
            _ctx.Add(component);
            _ctx.SaveChanges();
            return component;
        }
      
        public void DeleteComponent(Component comp)
        {
            _ctx.Remove(comp);
            _ctx.SaveChanges();
        }

        public Component GetComponent(int id)
        {
            Component? comp = new Component();
            comp = _ctx.Components.Include(x => x.Page).FirstOrDefault(x => x.Id == id);
            return comp;
        }

        public Component? GetComponentByIdentifier(ComponentIdDTO dto)
        {
            Component? comp = new Component();
            comp = _ctx.Components.Include(x => x.Page).ThenInclude(a => a.App).FirstOrDefault(x => x.ComponentIdentifier == dto.id);
            return comp;
        }

        public Component? GetComponentById(DynamicModelDTO model)
        {
          var component =  _ctx.Components.Include(x => x.Page).ThenInclude(i => i.App)
                            .Where(x => x.ComponentIdentifier == model.ComponentIdentifier && x.Page.Identifier == model.PageIdentifier && x.Page.App.Identifier == model.AppIdentifier)
                            .FirstOrDefault();
        return component;
        }

        public List<ComponentSDTO> GetComponents()
        {
            return _mapper.Map<List<ComponentSDTO>>(_ctx.Components.Include(x => x.Page).ToList());
        }

        public Component UpdateComponent(ComponentDTO model, int id)
        {
            var comp = _ctx.Components.First(p => p.Id == id);

            comp.ComponentIdentifier = model.ComponentIdentifier;
            comp.Name = model.Name;
            comp.Html = model.Html;
            comp.PageId = model.PageId;
            comp.UpdatedDate = DateTime.Now;
            _ctx.Entry(comp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _ctx.SaveChanges();
            return comp;
        }      
    }
}
