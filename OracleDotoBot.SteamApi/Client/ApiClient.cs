using Newtonsoft.Json;

namespace OracleDotoBot.SteamApi.Client
{
    public class ApiClient
    {
        public ApiClient(string token)
        {
            _httpClient = new HttpClient();
            _token = token;
        }

        private readonly HttpClient _httpClient;
        private readonly string _token;

        public async Task<(T? data, string error)> Request<T>(string url)
        {
            var uri = url + "?key=" + _token + "&partner=1";
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

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
