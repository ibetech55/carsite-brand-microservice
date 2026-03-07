namespace BrandMicroservice.src.Models.Dtos.Make
{
    public class ActivateMakeDto
    {
        public required string MakeCode { get; set; }

        public required string MakeName { get; set; }

        public required string Status { get; set; }
    }
}
