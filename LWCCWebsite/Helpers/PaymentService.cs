using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace LWCCWebsite.Helpers
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NmiPaymentResponse> ProcessPaymentAsync(PaymentRequestModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://your-api-site.com/api/payment/process", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<NmiPaymentResponse>(responseContent);
            }

            throw new Exception("Payment processing failed");
        }
    }
}

public class PaymentRequestModel
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CreditCardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVV { get; set; }
    public string Description { get; set; }
}

public class NmiPaymentResponse
{
    public string Response { get; set; }
    public string ResponseText { get; set; }
    public string TransactionId { get; set; }
}
