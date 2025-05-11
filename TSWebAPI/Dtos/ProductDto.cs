using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TSWebAPI.Dtos
{
    public record ProductDto
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; init; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; init; }
        [JsonProperty("data")]
        public Dictionary<string,object>? Data { get; init; }

    }
}
