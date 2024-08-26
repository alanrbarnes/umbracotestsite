using LWCCWebsite.Core.Models;
using System.Threading.Tasks;

namespace LWCCWebsite.Core.Interfaces
{
    public interface IPaymentService
    {
        public Task<NmiPaymentResponse> ProcessPaymentAsync(PaymentRequest request);
    }
}
