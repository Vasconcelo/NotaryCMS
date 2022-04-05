using System.ComponentModel.DataAnnotations;

namespace Notary.CMS.DataAccess.Models.DTOs
{
    public  class ComponentDTO
    {
        [Required(ErrorMessage = "The filed ComponentIdentifier is required")]
        public string ComponentIdentifier { get; set; }

        [Required(ErrorMessage = "The filed Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The filed Html is required")]
        public string Html { get; set; }

        [Required(ErrorMessage = "The filed PageId is required")]
        public int PageId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public ComponentDTO()
        {
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
        }
    }
}
