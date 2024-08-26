using LWCCWebsite.Core.Interfaces;
using LWCCWebsite.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LWCCWebsite.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _nmiApiUrl;
        private readonly string _securityKey;

        public PaymentService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _nmiApiUrl = configuration["Nmi:ApiUrl"];
            _securityKey = configuration["Nmi:SecurityKey"];
        }

        public async Task<NmiPaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            var nmiRequest = new Dictionary<string, string>
            {
                { "security_key", _securityKey },
                { "amount", request.Amount.ToString("0.00") },
                { "ccnumber", request.CreditCardNumber },
                { "ccexp", request.ExpiryDate },
                { "cvv", request.CVV },
                { "order_description", request.Description },
            };

            // Simulate an HTTP request
            var response = await _httpClient.PostAsync(_nmiApiUrl, new FormUrlEncodedContent(nmiRequest));
            var responseContent = await response.Content.ReadAsStringAsync();

            // Parse the response and create an NmiPaymentResponse object
            var nmiPaymentResponse = new NmiPaymentResponse
            {
                // Populate the response object based on the actual response content
            };

            return nmiPaymentResponse;
        }
    }
}