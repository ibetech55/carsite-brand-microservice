using System.Text.Json.Serialization;

namespace BrandMicroservice.src.Models.Dtos.Model
{
    public class ModelNameTotalDto
    {
        [JsonPropertyName("modelName")]
        public required string ModelName { get; set; }

        [JsonPropertyName("total")]
        public required int Total { get; set; }
    }
}
