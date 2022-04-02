using System;
using System.Collections.Generic;

namespace Notary.CMS.DataAccess.Models
{
    public partial class Page
    {
        public Page()
        {
            Components = new HashSet<Component>();
        }

        public int Id { get; set; }
        public string Identifier { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int AppId { get; set; }

        public virtual Application App { get; set; } = null!;
        public virtual ICollection<Component> Components { get; set; }
    }
}
