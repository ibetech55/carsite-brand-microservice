namespace BrandMicroservice.src.Models.Dtos.Make
{
    public class UpdateMake
    {
        public string? MakeName { get; set; }

        public string? Origin { get; set; }

        public string? MakeLogo { get; set; }

        public bool? Active { get; set; }

        public int? YearFounded { get; set; }

        public string? Company { get; set; }

        public string? MakeAbbreviation { get; set; }
    }
}
