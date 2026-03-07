namespace BrandMicroservice.src.Models.DbModels
{
    public class Model : BaseModel
    {
        public required string ModelName { get; set; }

        public required Guid MakeId { get; set; }

        public required bool Active { get; set; }

        public required string BodyType { get; set; }

        public required int YearFounded { get; set; }

        public required string ModelCode { get; set; }

        public Make? Makes { get; set; }
    }
}
