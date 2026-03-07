namespace BrandMicroservice.src.Models.DbModels
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
