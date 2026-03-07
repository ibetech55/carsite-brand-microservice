namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class UpdateModelDto
    {
        public required string ModelName { get; set; }

        public required bool Active { get; set; }

        public required string BodyType { get; set; }

        public required int YearFounded { get; set; }
    }
}
