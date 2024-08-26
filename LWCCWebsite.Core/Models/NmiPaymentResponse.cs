using System.Linq;

namespace LWCCWebsite.Core.Models
{
    public class NmiPaymentResponse
    {
        public string Response { get; set; }
        public string ResponseText { get; set; }
        public string TransactionId { get; set; }

        public static NmiPaymentResponse FromQueryString(string queryString)
        {
            var keyValuePairs = queryString.Split('&')
                .Select(part => part.Split('='))
                .ToDictionary(split => split[0], split => split[1]);

            return new NmiPaymentResponse
            {
                Response = keyValuePairs["response"],
                ResponseText = keyValuePairs["responsetext"],
                TransactionId = keyValuePairs["transactionid"],
            };
        }
    }
}
