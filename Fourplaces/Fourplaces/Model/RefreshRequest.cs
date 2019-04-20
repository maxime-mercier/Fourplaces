using Newtonsoft.Json;

namespace Fourplaces.Model
{
    public class RefreshRequest
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}