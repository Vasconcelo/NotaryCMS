using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

namespace Notary.CMS.DataAccess.Interfaces
{
    public  interface IPageRepository
    {
        List<PageSDTO> GetPages(); 
        Page GetPage(int id);
        Page CreatePage(Page page);
        Page UpdatePage(PageDTO model, int id);
        void DeletePage(int id);
    }
}
