using OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Login
{
    public class TwitterAuthService : ITwitterAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITwitterAuthOptions _options;

        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private const string AccessTokenSecret = "vHlwdwxkk3c3LjLUqx5gOM2JFwAcRsXjRzzsuyjbszTCv";
        private const string AccessToken = "800326740711047169-Swm22fdkjwjq7LDtu6zb3H3rKmxlAdb";
        private const string CallbackUrl = "https://localhost:7154/users/twitter";
        private const string SignatureMethod = "HMAC-SHA1";

        public TwitterAuthService(
            IHttpClientFactory httpClientFactory,
            ITwitterAuthOptions options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _consumerKey = _options.AppId;
            _consumerSecret = _options.AppSecret;
        }

        public async Task<RequestTokenResponse> GetRequestToken()
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.twitter.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TimeSpan timestamp = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            Dictionary<string, string> oauth = new Dictionary<string, string>
            {
                ["oauth_callback"] = CallbackUrl,
                ["oauth_consumer_key"] = _consumerKey,
                ["oauth_nonce"] = new string(nonce.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray()),
                ["oauth_signature_method"] = SignatureMethod,
                ["oauth_timestamp"] = Convert.ToInt64(timestamp.TotalSeconds).ToString(),
                ["oauth_token"] = AccessToken,
                ["oauth_version"] = "1.0",
            };

            string[] parameterCollectionValues = oauth.Select(parameter =>
                    Uri.EscapeDataString(parameter.Key) + "=" +
                    Uri.EscapeDataString(parameter.Value))
                .OrderBy(kv => kv)
                .ToArray();
            string parameterCollection = string.Join("&", parameterCollectionValues);

            string baseString = "POST";
            baseString += "&";
            baseString += Uri.EscapeDataString(client.BaseAddress + "oauth/request_token");
            baseString += "&";
            baseString += Uri.EscapeDataString(parameterCollection);

            string signingKey = Uri.EscapeDataString(_consumerSecret);
                
            signingKey += "&" + Uri.EscapeDataString(AccessTokenSecret);
            HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
                

            oauth = new Dictionary<string, string>
            {
                ["oauth_callback"] = CallbackUrl,
                ["oauth_consumer_key"] = _consumerKey,
                ["oauth_nonce"] = new string(nonce.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray()),
                ["oauth_signature"] = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString))),
                ["oauth_signature_method"] = SignatureMethod,
                ["oauth_timestamp"] = Convert.ToInt64(timestamp.TotalSeconds).ToString(),
                ["oauth_token"] = AccessToken,
                ["oauth_version"] = "1.0",
            };

            string headerString = "OAuth ";
            string[] headerStringValues = oauth.Select(parameter =>
                    Uri.EscapeDataString(parameter.Key) + "=" + "\"" +
                    Uri.EscapeDataString(parameter.Value) + "\"")
                .ToArray();
            headerString += string.Join(", ", headerStringValues);

            client.DefaultRequestHeaders.Add("Authorization", headerString);

            HttpResponseMessage response = await client.PostAsJsonAsync("oauth/request_token", "");
            var responseString = await response.Content.ReadAsStringAsync();

            var data = responseString.Split("&");

            return new RequestTokenResponse
            {
                Token = data[0],
                TokenSecret = data[1],
                IsCallbackConfirmed = data[2],
            };
        }

        public async Task<UserModelDto> GetAccessToken(string oauth_request, string oauth_verifier)
        {
            var client = _httpClientFactory.CreateClient("twitter");
            var consumerKey = _options.AppId;
            var consumerSecret = _options.AppSecret;

            var accessTokenResponse = new UserModelDto();

            client.DefaultRequestHeaders.Accept.Clear();

            var oauthClient = new OAuthRequest
            {
                Method = "POST",
                Type = OAuthRequestType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                RequestUrl = "https://api.twitter.com/oauth/access_token",
                Token = oauth_request,
                Version = "1.0a",
                Realm = "twitter.com"
            };

            string auth = oauthClient.GetAuthorizationHeader();

            client.DefaultRequestHeaders.Add("Authorization", auth);


            try
            {
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string, string>("oauth_verifier", oauth_verifier)
                });

                using (var response = await client.PostAsync(oauthClient.RequestUrl, content))
                {
                    response.EnsureSuccessStatusCode();

                    var responseString = response.Content.ReadAsStringAsync()
                                               .Result.Split("&");

                    accessTokenResponse = new UserModelDto
                    {
                        Token = responseString[0].Split("=")[1],
                        TokenSecret = responseString[1].Split("=")[1],
                        UserId = responseString[2].Split("=")[1],
                        Username = responseString[3].Split("=")[1]
                    };

                }
            }
            catch (Exception ex)
            {


            }

            return accessTokenResponse;

        }
    }

    public class UserModelDto
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }

    }
}
