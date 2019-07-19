using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGatewayApi.Controllers;
using PaymentGatewayApi.Model;
using PaymentGatewayApi.Service;
using Xunit;

namespace PaymentGatewayApi.UnitTests.Controller
{
    public class PaymentGatewayControllerShould
    {
        private Mock<IPaymentGatewayService> paymentGatewayServiceMock;

        public PaymentGatewayControllerShould()
        {
            paymentGatewayServiceMock = new Mock<IPaymentGatewayService>();
        }

        [Fact]
        public async Task Return_BadRequest_When_PaymentRequest_Is_Invalid()
        {
            // Arrange
            var paymentGatewayController = new PaymentGatewayController(paymentGatewayServiceMock.Object);

            // Act
            PaymentRequest invalidPaymentRequest = new PaymentRequest("", "", 0m, "", 0);
            var response = await paymentGatewayController.Post(invalidPaymentRequest) as BadRequestObjectResult;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)response.StatusCode);
            Assert.Equal("Invalid payment request", response.Value);
        }

        [Fact]
        public async Task Call_PaymentGatewayService_To_Process_Payment_When_Handling_Request()
        {
            // Arrange
            var paymentGatewayController = new PaymentGatewayController(paymentGatewayServiceMock.Object);
            var paymentRequest = CreatePaymentRequest();

            // Act
            await paymentGatewayController.Post(paymentRequest);

            // Assert
            paymentGatewayServiceMock.Verify(service => service.SendPayment(paymentRequest), Times.Once);
        }

        private static PaymentRequest CreatePaymentRequest()
        {
            string cardNumber = "4485063526474709";
            string expiryDate = "01/20";
            decimal amount = 20.00m;
            string currency = "GBP";
            int cvv = 123;
            var paymentRequest = new PaymentRequest(cardNumber, expiryDate, amount, currency, cvv);
            return paymentRequest;
        }
    }
}
