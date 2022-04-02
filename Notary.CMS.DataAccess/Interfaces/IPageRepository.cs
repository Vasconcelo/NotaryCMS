using Notary.Api.DataAccess.Models;
using Notary.Api.DataAccess.Models.DTOs;

namespace Notary.Api.DataAccess.Interfaces
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
