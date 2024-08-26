using LWCCWebsite.Core.Interfaces;
using LWCCWebsite.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LWCCWebsite.Controllers
{
    public class PaymentApiController : Controller
    {
        private readonly IPaymentApiService _paymentApiService;

        public PaymentApiController(IPaymentApiService paymentApiService)
        {
            _paymentApiService = paymentApiService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(NmiPaymentApiRequest request)
        {
            try
            {
                var response = await _paymentApiService.ProcessPayment(request);

                if (response.Status == "1")
                {
                    return Ok(new { success = true, transactionId = response.TransactionId });
                }
                else
                {
                    return BadRequest(new { success = false, message = response.Status });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
