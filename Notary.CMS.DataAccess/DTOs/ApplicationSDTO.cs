namespace Notary.CMS.DataAccess.Models.DTOs
{
    public  class ApplicationSDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public  ICollection<PageSDTO> Pages { get; set; }
    }
}
