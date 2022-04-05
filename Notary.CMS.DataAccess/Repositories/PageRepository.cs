using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notary.CMS.DataAccess.Interfaces;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

namespace Notary.CMS.DataAccess.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly NotaryCMSDBContext _ctx;
        private readonly IMapper _mapper;

        public PageRepository(NotaryCMSDBContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public Page CreatePage(Page page)
        {          
            _ctx.Entry(page).State = EntityState.Added;
            _ctx.Add(page);
            _ctx.SaveChanges();
            return page;
        }

        public void DeletePage(int id)
        {
            var page = _ctx.Pages.Find(id);
            if (page != null)
            {
                _ctx.Remove(page);
                _ctx.SaveChanges();
            }     
        }      

        public List<PageSDTO> GetPages()
        {
            return _mapper.Map<List<PageSDTO>>(_ctx.Pages.Include(x => x.App).ToList());
        }

        public Page UpdatePage(PageDTO model, int id)
        {
            var page = _ctx.Pages.First(p => p.Id == id);

            page.Identifier = model.Identifier;
            page.Name = model.Name;
            page.Description = model.Description;
            page.AppId = model.AppId;
            page.UpdatedDate = DateTime.Now;
            _ctx.Entry(page).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _ctx.SaveChanges();
            return page;
        }

        public Page GetPage(int id)
        {
            Page? page = new Page();
            page = _ctx.Pages.Include(x => x.Components).FirstOrDefault(x => x.Id == id);            
            return page;            
        }
        
    }
}
