using BrandMicroservice.src.Models.Dtos.Shared;

namespace BrandMicroservice.src.Models.Dtos.MakeDTOs
{
    public class MakeQuery : BaseQuery
    {
        public string? MakeName { get; set; }

        public string? MakeCode { get; set; }
    }
}
