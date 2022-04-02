using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Api.DataAccess.Models.DTOs
{
    public  class ComponentSDTO
    {
        public int Id { get; set; }
        public string ComponentIdentifier { get; set; }
        public string Name { get; set; }
        public string Html { get; set; } 
        public int PageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public  PageSDTO Page { get; set; } 
    }
}
