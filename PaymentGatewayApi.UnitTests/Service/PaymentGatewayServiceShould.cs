using System;
using System.Threading.Tasks;
using Moq;
using PaymentGatewayApi.Api;
using PaymentGatewayApi.Api.Dto;
using PaymentGatewayApi.Logging;
using PaymentGatewayApi.Mapper;
using PaymentGatewayApi.Model;
using PaymentGatewayApi.Service;
using Xunit;

namespace PaymentGatewayApi.UnitTests.Service
{
    public class PaymentGatewayServiceShould
    {
        private Mock<IBankApiClient> bankApiMock;
        private Mock<IBankPaymentMapper> bankPaymentMapperMock;
        private PaymentRequest paymentRequest;
        private PaymentGatewayService paymentGatewayService;
        private Guid paymentReference;
        private BankPaymentRequest bankPaymentRequest;
        private Mock<IApplicationLogger> applicationLoggerMock;

        public PaymentGatewayServiceShould()
        {
            paymentReference = Guid.NewGuid();
            paymentRequest = CreatePaymentRequest();
            bankPaymentRequest = new BankPaymentRequest("4485063526474709", "01/20", 20.00m, "GBP", 123);
            bankApiMock = new Mock<IBankApiClient>();
            var bankPaymentDetail = new BankPaymentDetail(paymentReference, "Accepted", "4485063526474709", "01/20", 20.00m, "GBP", 123);
            var bankPaymentResponse = new BankPaymentResponse(paymentReference, "Accepted");
            var paymentResponse = new PaymentResponse(paymentReference, "Accepted");
            var paymentDetail = new PaymentDetail(paymentReference, "Accepted", "4***********4709", "01/20", 20.00m, "GBP", 123);
            
            bankApiMock.Setup(service => service.Process(It.IsAny<BankPaymentRequest>()))
                .Returns(Task.FromResult(bankPaymentResponse));
            bankApiMock.Setup(service => service.GetPaymentDetail(paymentReference)).Returns(Task.FromResult(bankPaymentDetail));
            bankPaymentMapperMock = new Mock<IBankPaymentMapper>();
            bankPaymentMapperMock.Setup(mapper => mapper.MapRequest(It.IsAny<PaymentRequest>())).Returns(bankPaymentRequest);
            bankPaymentMapperMock.Setup(mapper => mapper.MapResponse(It.IsAny<BankPaymentResponse>()))
                .Returns(paymentResponse);
            bankPaymentMapperMock.Setup(mapper => mapper.MapDetail(It.IsAny<BankPaymentDetail>()))
                .Returns(paymentDetail);
            applicationLoggerMock = new Mock<IApplicationLogger>();
            paymentGatewayService = new PaymentGatewayService(bankApiMock.Object, bankPaymentMapperMock.Object, applicationLoggerMock.Object);
        }

        [Fact]
        public async Task Call_BankApiClient_When_Sending_PaymentRequest()
        {
            // Act
            await paymentGatewayService.SendPayment(paymentRequest);

            // Assert
            bankApiMock.Verify(service => service.Process(It.IsAny<BankPaymentRequest>()), Times.Once);
        }

        [Fact]
        public async Task Return_PaymentResponse_When_PaymentRequest_Is_Sent()
        {
            // Arrange
            var bankPaymentResponse = new BankPaymentResponse(paymentReference, "Accepted");
            bankApiMock.Setup(service => service.Process(bankPaymentRequest))
                .Returns(Task.FromResult(bankPaymentResponse));

            // Act
            var response = await paymentGatewayService.SendPayment(paymentRequest);

            // Assert
            Assert.IsType<PaymentResponse>(response);
            Assert.Equal(paymentReference, response.PaymentReference);
            Assert.Equal("Accepted", response.Status);
        }

        [Fact]
        public async Task Return_Accepted_PaymentResponse_When_PaymentRequest_Is_Successful()
        {
            // Arrange
            var bankPaymentResponse = new BankPaymentResponse(paymentReference, "Accepted");
            bankApiMock.Setup(service => service.Process(It.IsAny<BankPaymentRequest>()))
                .Returns(Task.FromResult(bankPaymentResponse));

            // Act
            var response = await paymentGatewayService.SendPayment(paymentRequest);

            // Assert
            Assert.IsType<PaymentResponse>(response);
            Assert.Equal("Accepted", response.Status);
        }
        
        [Fact]
        public async Task Return_Declined_PaymentResponse_When_PaymentRequest_Is_Unsuccessful()
        {
            // Arrange
            var bankPaymentResponse = new BankPaymentResponse(paymentReference, "Declined");
            bankApiMock.Setup(service => service.Process(It.IsAny<BankPaymentRequest>()))
                .Returns(Task.FromResult(bankPaymentResponse));
            var unsuccessfulPaymentResponse = new PaymentResponse(paymentReference, "Declined");
            bankPaymentMapperMock.Setup(mapper => mapper.MapResponse(It.IsAny<BankPaymentResponse>()))
                .Returns(unsuccessfulPaymentResponse);

            // Act
            var response = await paymentGatewayService.SendPayment(paymentRequest);

            // Assert
            Assert.IsType<PaymentResponse>(response);
            Assert.Equal("Declined", response.Status);
        }

        [Fact]
        public async Task Call_BankPaymentMapper_When_Sending_PaymentRequest()
        {
            // Act
            var response = await paymentGatewayService.SendPayment(paymentRequest);

            // Assert
            bankPaymentMapperMock.Verify(mapper => mapper.MapRequest(It.IsAny<PaymentRequest>()), Times.Once);
            bankPaymentMapperMock.Verify(mapper => mapper.MapResponse(It.IsAny<BankPaymentResponse>()), Times.Once);
        }

        [Fact]
        public async Task Call_BankPaymentMapper_When_Getting_PaymentDetail()
        {
            // Act
            var response = await paymentGatewayService.GetPaymentDetail(paymentReference);

            // Assert
            bankPaymentMapperMock.Verify(mapper => mapper.MapDetail(It.IsAny<BankPaymentDetail>()), Times.Once);
        }

        [Fact]
        public async Task Call_BankApiClient_When_Getting_PaymentDetail()
        {
            // Act
            await paymentGatewayService.GetPaymentDetail(paymentReference);

            // Assert
            bankApiMock.Verify(service => service.GetPaymentDetail(paymentReference));
        }

        [Fact]
        public async Task Return_PaymentDetail_When_Retrieiving_PaymentDetail()
        {
            // Act
            var paymentDetail = await paymentGatewayService.GetPaymentDetail(paymentReference);

            // Assert
            Assert.IsType<PaymentDetail>(paymentDetail);
        }

        [Fact]
        public async Task Log_When_PaymentRequest_Has_Been_Processed()
        {
            // Act
            await paymentGatewayService.SendPayment(paymentRequest);

            // Assert
            applicationLoggerMock.Verify(logger => logger.LogInformation($"Finished processing payment request: {paymentReference}"));
        }

        [Fact]
        public async Task Log_When_PaymentReference_Has_Been_Processed()
        {
            // Act
            await paymentGatewayService.GetPaymentDetail(paymentReference);

            // Assert
            applicationLoggerMock.Verify(logger => logger.LogInformation($"Finished handling request for payment reference: {paymentReference}"));
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
