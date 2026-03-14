namespace BrandMicroservice.src.Models.DbModels
{
    public class LookupValues
    {
        public required Guid Id { get; set; }
        
        public required Guid FormId { get; set; }

        public required Guid FieldId { get; set; }

        public required string Value { get; set; }

        public required string Option { get; set; }
    }
}
