using System;
using System.Threading.Tasks;
using BankApiStub.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BankApiStub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StubController : ControllerBase
    {
        [HttpGet("{paymentReference}")]
        public async Task<IActionResult> Get(Guid paymentReference)
        {
            await Task.Delay(100);
            var bankPaymentDetail = new BankPaymentDetail(paymentReference, "Accepted", "4485063526474709", "01/20", 20.00m, "GBP", 123);
            
            return Ok(bankPaymentDetail);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(BankPaymentRequest bankPaymentRequest)
        {
            await Task.Delay(100);
            var status = bankPaymentRequest.Amount < 50 ? "Accepted" : "Declined";
            var bankResponse = new BankPaymentResponse(Guid.NewGuid(), status);
            return Ok(bankResponse);
        }
    }
}
