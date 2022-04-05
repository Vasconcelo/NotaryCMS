namespace Notary.CMS.DataAccess.Models
{
    public partial class Component
    {
        public int Id { get; set; }
        public string ComponentIdentifier { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Html { get; set; } = null!;
        public int PageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Page Page { get; set; } = null!;
    }
}
