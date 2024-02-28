using Newtonsoft.Json;
using System.Text;

namespace OracleDotoBot.StratzApiParser.Client
{
    public class ApiClient
    {
        public ApiClient(string baseUrl, string token)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            _authorizationToken = token;
        }

        private readonly HttpClient _httpClient;

        private readonly string _authorizationToken;

        public async Task<(T? data, string error)> Request<T>(string content)
        {
            var queryObject = new
            {
                query = content
            };

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(queryObject), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {_authorizationToken}");

            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                    return (default, response.StatusCode.ToString());

                var responseContent = await response.Content.ReadAsStringAsync();

                var deserialized = JsonConvert.DeserializeObject<T>(responseContent);
                return (deserialized, "");
            }
        }
    }
}
