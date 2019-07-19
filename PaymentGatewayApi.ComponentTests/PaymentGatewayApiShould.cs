using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGatewayApi.Model;
using Xunit;

namespace PaymentGatewayApi.ComponentTests
{
    public class PaymentGatewayApiShould
    {
        public PaymentGatewayApiShould()
        {
            //TODO: Startup the PaymentGatewayApi and BankApiStub before running the testss => dotnet test .\PaymentGatewayApi.ComponentTests\.
            //TODO: Going forward, create a dockerfile for the PaymentGatewayApi, BankApiStub & this project and use docker-compose to spin-up the 2 apis before running the tests
        }

        [Fact]
        public async Task Return_An_Accepted_PaymentResponse_When_A_PaymentRequest_Is_Sent()
        {
            // Arrange
            var paymentGatewayApiClient = new HttpClient(){BaseAddress = new Uri("https://localhost:44358/")};
            var stringContent = CreateHttpContent(CreateAcceptedPaymentRequest());

            // Act
            var response =
                await paymentGatewayApiClient.PostAsync("api/paymentgateway", stringContent);
            
            //Assert
            var paymentResponseContent = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(paymentResponseContent);
            Assert.Equal("Accepted", paymentResponse.Status);
        }

        [Fact]
        public async Task Return_A_Declined_PaymentResponse_When_A_PaymentRequest_Is_Sent()
        {
            // Arrange
            var paymentGatewayApiClient = new HttpClient(){BaseAddress = new Uri("https://localhost:44358/")};
            var stringContent = CreateHttpContent(CreateDeclinedPaymentRequest());

            // Act
            var response =
                await paymentGatewayApiClient.PostAsync("api/paymentgateway", stringContent);
            
            //Assert
            var paymentResponseContent = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(paymentResponseContent);
            Assert.Equal("Declined", paymentResponse.Status);
        }

        [Fact]
        public async Task Return_PaymentDetail_When_A_PaymentReference_Is_Sent()
        {
            // Arrange
            var paymentGatewayApiClient = new HttpClient(){BaseAddress = new Uri("https://localhost:44358/")};
            var paymentReference = Guid.NewGuid();

            // Act
            var response =
                await paymentGatewayApiClient.GetAsync($"api/paymentgateway/{paymentReference}");

            // Assert
            var paymentDetailContent = await response.Content.ReadAsStringAsync();
            var paymentDetail = JsonConvert.DeserializeObject<PaymentDetail>(paymentDetailContent);
            Assert.IsAssignableFrom<PaymentDetail>(paymentDetail);
        }

        private static StringContent CreateHttpContent(PaymentRequest paymentRequest)
        {
            var serialisedPaymentRequest = JsonConvert.SerializeObject(paymentRequest);
            var stringContent = new StringContent(serialisedPaymentRequest, Encoding.UTF8, "application/json");
            return stringContent;
        }

        private static PaymentRequest CreateAcceptedPaymentRequest()
        {
            string cardNumber = "4485063526474709";
            string expiryDate = "01/20";
            decimal amount = 20.00m;
            string currency = "GBP";
            int cvv = 123;
            var paymentRequest = new PaymentRequest(cardNumber, expiryDate, amount, currency, cvv);
            return paymentRequest;
        }

        private static PaymentRequest CreateDeclinedPaymentRequest()
        {
            string cardNumber = "4485063526474709";
            string expiryDate = "01/20";
            decimal amount = 100.00m;
            string currency = "GBP";
            int cvv = 123;
            var paymentRequest = new PaymentRequest(cardNumber, expiryDate, amount, currency, cvv);
            return paymentRequest;
        }
    }
}
