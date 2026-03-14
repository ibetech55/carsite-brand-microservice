namespace BrandMicroservice.src.Models.DbModels
{
    public class FormField
    {
        public required Guid Id { get; set; }

        public required string? FieldLabel { get; set; }

        public required string ControlName { get; set; }

        public required string? FieldType { get; set; }

        public required string? Description { get; set; }

        public required string? HelpText { get; set; }

        public required string? Source { get; set; }

        public required Guid LookupValueId { get; set; }

        public required Guid FormId {  get; set; }

        public Form? Form { get; set; }

        public bool? Multiple { get; set; } = true;

        public bool IsActive { get; set; } = true;
    }
}
