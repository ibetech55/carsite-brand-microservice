namespace BrandMicroservice.src.Models.Dtos.Shared
{
    public class FileData
    {
        public required string Name { get; set; }

        public required string Type { get; set; }

        public required int LastModified { get; set; }

        public required int Size { get; set; }
    }
}
