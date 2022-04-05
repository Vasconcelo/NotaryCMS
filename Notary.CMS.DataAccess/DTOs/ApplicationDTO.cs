using System.ComponentModel.DataAnnotations;

namespace Notary.CMS.DataAccess.Models.DTOs
{
    public class ApplicationDTO
    {
        [Required(ErrorMessage = "The filed Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The filed Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The filed Identifier is required")]
        public string Identifier { get; set; }

        public DateTime CreatedAt { get; set; }
           
        public DateTime UpdatedAt { get; set; }

        public ApplicationDTO()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }


    }
}
