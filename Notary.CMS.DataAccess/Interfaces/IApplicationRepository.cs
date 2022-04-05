using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

namespace Notary.CMS.DataAccess.Interfaces
{
    public  interface IApplicationRepository
    {
        Application CreateApplication(Application app);
        void DeleteApplication(Application app);
        Application UpdateApplication(ApplicationDTO model, int id);
        List<ApplicationSDTO> GetApplications();

        Application GetApplication(int id);

    }
}
