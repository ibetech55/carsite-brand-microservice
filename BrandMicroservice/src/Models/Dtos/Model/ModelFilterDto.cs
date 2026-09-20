namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class ModelFilterDto
    {
        public required string MakeName { get; set; }
        public required string ModelCode { get; set; }
        public required string ModelName { get; set; }
        public required int Total { get; set; }

    }
}
