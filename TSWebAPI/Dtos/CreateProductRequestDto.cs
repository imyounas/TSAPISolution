using Newtonsoft.Json;

namespace TSWebAPI.Dtos
{
    public record CreateProductRequestDto
    {
        [JsonProperty("name")]
        public string Name { get; init; } =string.Empty;
        [JsonProperty("data")]
        public Dictionary<string, object>? Data { get; init; }
    }
}
