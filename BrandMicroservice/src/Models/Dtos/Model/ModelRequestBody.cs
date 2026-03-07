namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class ModelRequestBody
    {
        public required string ModelName { get; set; }

        public required string MakeCode { get; set; }

        public required List<string> BodyType { get; set; }

        public required int YearFounded { get; set; }
    }
}