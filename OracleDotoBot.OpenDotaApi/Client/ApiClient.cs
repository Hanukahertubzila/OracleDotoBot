using Newtonsoft.Json;

namespace OracleDotoBot.OpenDotaApiParser.Client
{
    public class ApiClient
    {
        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        private readonly HttpClient _httpClient;

        public async Task<(T? data, string error)> Request<T>(string url)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
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
