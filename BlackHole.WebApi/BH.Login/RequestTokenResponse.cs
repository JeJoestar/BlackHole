using Newtonsoft.Json;

namespace BlackHole.Login
{
    public class RequestTokenResponse
    {
        [JsonProperty("oauth_token")]
        public string Token { get; set; }

        [JsonProperty("oauth_token_secret")]
        public string TokenSecret { get; set; }

        [JsonProperty("oauth_callback_confirmed")]
        public string IsCallbackConfirmed { get; set; }
    }
}