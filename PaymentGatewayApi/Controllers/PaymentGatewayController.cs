using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayApi.Model;
using PaymentGatewayApi.Service;

namespace PaymentGatewayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPaymentGatewayService _paymentGatewayService;

        public PaymentGatewayController(IPaymentGatewayService paymentGatewayService)
        {
            _paymentGatewayService = paymentGatewayService;
        }

        [HttpGet("{paymentReference}")]
        public async Task<IActionResult> Get(Guid paymentReference)
        {
            var paymentDetail = await _paymentGatewayService.GetPaymentDetail(paymentReference);
            return Ok(paymentDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PaymentRequest paymentRequest)
        {
            if (!paymentRequest.Validate())
            {
                return BadRequest("Invalid payment request");
            }

            var paymentResponse = await _paymentGatewayService.SendPayment(paymentRequest);

            return Ok(paymentResponse);
        }
    }
}
