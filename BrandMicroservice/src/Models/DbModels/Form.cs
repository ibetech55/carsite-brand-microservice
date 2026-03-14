namespace BrandMicroservice.src.Models.DbModels
{
    public class Form
    {
        public required Guid Id { get; set; }
        
        public required string FormName { get; set; }

        public required string FormType { get; set; }

        public required string? FormDescription { get; set; }
    }

}
