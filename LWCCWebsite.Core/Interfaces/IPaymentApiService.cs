using LWCCWebsite.Core.Models;
using System.Threading.Tasks;

namespace LWCCWebsite.Core.Interfaces
{
    public interface IPaymentApiService
    {
        Task<NmiPaymentApiResponse> ProcessPayment(NmiPaymentApiRequest request);
    }
}
