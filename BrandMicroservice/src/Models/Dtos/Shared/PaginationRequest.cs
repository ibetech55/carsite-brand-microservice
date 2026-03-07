namespace BrandMicroservice.src.Models.Dtos.Shared
{
    public class PaginationRequest<T>
    {
        public required int Page { get; set; }

        public required int Limit { get; set; }

        public  T? Data { get; set; }
    }
}
