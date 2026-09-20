namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class ModelFilterListDto
    {
        public required List<ModelFilterDto> ModelFilterData { get; set; }

        public required string MakeName { get; set; }
    }
}
