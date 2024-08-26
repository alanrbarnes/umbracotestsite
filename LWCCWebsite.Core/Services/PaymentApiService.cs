using LWCCWebsite.Core.Interfaces;
using LWCCWebsite.Core.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LWCCWebsite.Core.Services
{
    public class PaymentApiService : IPaymentApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public PaymentApiService(HttpClient httpClient, string apiUrl, string apiKey)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }

        public async Task<NmiPaymentApiResponse> ProcessPayment(NmiPaymentApiRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiUrl)
            {
                Headers = { { "Authorization", $"Basic {_apiKey}" } },
                Content = content
            };

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NmiPaymentApiResponse>(responseContent);
        }

    }
}
