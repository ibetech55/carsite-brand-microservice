namespace BrandMicroservice.src.Models.Dtos.Shared
{
    public class Pagination<T>
    {
        public required int Page { get; set; }

        public required int Limit { get; set; }

        public required int TotalCount { get; set; }

        public required List<T> Data { get; set; }
    }
}
