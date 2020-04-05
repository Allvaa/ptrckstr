using Newtonsoft.Json;

namespace ptrckstr
{
    public struct Config
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
    }
}
