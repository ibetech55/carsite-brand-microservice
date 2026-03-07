namespace BrandMicroservice.src.Models.Dtos.Shared
{
    public class QueryParams<T>
    {
        public int? Limit {  get; set; }

        public int? Page { get; set; }

        public T Data { get; set; }
    }
}
