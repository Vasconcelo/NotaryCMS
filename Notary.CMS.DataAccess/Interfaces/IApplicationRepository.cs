using Notary.Api.DataAccess.Models;
using Notary.Api.DataAccess.Models.DTOs;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
