using BrandMicroservice.src.Models.Dtos.Shared;

namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class ModelQuery : BaseQuery
    {
        public string? ModelName { get; set; }

        public string? MakeName { get; set; }

        public string? ModelCode { get; set; }

        public string? Active { get; set; }

        public string? BodyType { get; set; }

        public int? YearFounded { get; set; }
    }
}
