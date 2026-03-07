using BrandMicroservice.src.Models.Dtos.Shared;

namespace BrandMicroservice.src.Models.Dtos.Make
{
    public class MakeRequestBody
    {
        public required string MakeName { get; set; }

        public required string Origin { get; set; }

        public required FileData MakeLogo { get; set; }

        public required int YearFounded { get; set; }

        public required string Company { get; set; }

        public required string MakeAbbreviation { get; set; }
    }
}
