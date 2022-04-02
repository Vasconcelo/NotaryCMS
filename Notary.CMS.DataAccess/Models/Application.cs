using System;
using System.Collections.Generic;

namespace Notary.CMS.DataAccess.Models
{
    public partial class Application
    {
        public Application()
        {
            Pages = new HashSet<Page>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Identifier { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
    }
}
