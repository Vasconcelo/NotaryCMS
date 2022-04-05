namespace Notary.CMS.DataAccess.Models.DTOs
{
    public  class PageSDTO
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int AppId { get; set; }

        public  ApplicationSDTO App { get; set; }
        public  ICollection<ComponentSDTO> Components { get; set; }
    }
}
