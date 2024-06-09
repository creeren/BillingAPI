using BillingAPI.Helper;
using BillingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BillingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly BillingHelper _billingHelper;

        public BillingController(BillingHelper billingHelper)
        {
            _billingHelper = billingHelper;
        }

        /// <summary>
        /// Processes an order and handles payment.
        /// </summary>
        /// <param name="order">The order to process.</param>
        /// <returns>Returns the receipt if the status code is 200, otherwise returns the Response with error</returns>
        [HttpPost("process-order")]
        public async Task<IActionResult> ProcessOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = new BillingResponse
                {
                    Receipt = await _billingHelper.ProcessOrderAsync(order)
                };

                return Ok(response);                             
            }
            catch (StripeException ex)
            {
                return BadRequest(new Response()
                {
                    StatusCode = (int)ex.HttpStatusCode,
                    Message = ex.StripeError?.Message,
                    ErrorCode = ex.StripeError?.Code ?? "UnknownError"
                });                   
            }
        }
    }
}
