using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Api.DataAccess.Models.DTOs
{
    public  class DynamicModelDTO
    {
        public string PageIdentifier
        {
            get; set;
        }

        public string ComponentIdentifier
        {
            get; set;
        }

        public string AppIdentifier
        {
            get; set;
        }
    }
}
