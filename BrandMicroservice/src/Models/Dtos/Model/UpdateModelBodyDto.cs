namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class UpdateModelBodyDto
    {
        public string? ModelName { get; set; }

        public bool? Active { get; set; }

        public List<string>? BodyType { get; set; }

        public int? YearFounded { get; set; }
    }
}
