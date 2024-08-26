namespace LWCCWebsite.Core.Models
{
    public class NmiPaymentApiRequest
    {
        public string Amount { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
    }
}
