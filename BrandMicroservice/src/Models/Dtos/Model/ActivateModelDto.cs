namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class ActivateModelDto
    {
        public required string ModelName { get; set; }

        public required string ModelCode { get; set; }

        public required string Status {  get; set; }
    }
}
