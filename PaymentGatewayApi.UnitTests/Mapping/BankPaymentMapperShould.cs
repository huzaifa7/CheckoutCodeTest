using System;
using PaymentGatewayApi.Api.Dto;
using PaymentGatewayApi.Mapper;
using PaymentGatewayApi.Model;
using Xunit;

namespace PaymentGatewayApi.UnitTests.Mapping
{
    public class BankPaymentMapperShould
    {
        [Fact]
        public void Should_Map_PaymentRequest_To_BankPaymentRequest()
        {
            // Arrange
            var paymentRequest = new PaymentRequest("4485063526474709", "01/20", 20.00m, "GBP", 123);
            var bankPaymentMapper = new BankPaymentMapper();

            // Act
            var bankPaymentRequest = bankPaymentMapper.MapRequest(paymentRequest);

            // Assert
            Assert.Equal(paymentRequest.CardNumber, bankPaymentRequest.CardNumber);
            Assert.Equal(paymentRequest.ExpiryDate, bankPaymentRequest.ExpiryDate);
            Assert.Equal(paymentRequest.Amount, bankPaymentRequest.Amount);
            Assert.Equal(paymentRequest.Currency, bankPaymentRequest.Currency);
            Assert.Equal(paymentRequest.Cvv, bankPaymentRequest.Cvv);
        }
        
        [Fact]
        public void Should_Map_BankPaymentResponse_To_PaymentResponse()
        {
            // Arrange
            var bankPaymentResponse = new BankPaymentResponse(Guid.NewGuid(), "Accepted");
            var bankPaymentMapper = new BankPaymentMapper();

            // Act
            var paymentResponse = bankPaymentMapper.MapResponse(bankPaymentResponse);

            // Assert
            Assert.Equal(bankPaymentResponse.PaymentReference, paymentResponse.PaymentReference);
            Assert.Equal(bankPaymentResponse.Status, paymentResponse.Status);
        }
        
        [Fact]
        public void Should_Map_BankPaymentDetail_To_PaymentDetail()
        {
            // Arrange
            var bankPaymentDetail = new BankPaymentDetail(Guid.NewGuid(), "Accepted", "4485063526474709", "01/20", 20.00m, "GBP", 123);
            var bankPaymentMapper = new BankPaymentMapper();

            // Act
            var paymentDetail = bankPaymentMapper.MapDetail(bankPaymentDetail);

            // Assert
            Assert.Equal(bankPaymentDetail.PaymentReference, paymentDetail.PaymentReference);
            Assert.Equal(bankPaymentDetail.Status, paymentDetail.Status);
            Assert.Equal("4***********4709", paymentDetail.CardNumber);
            Assert.Equal(bankPaymentDetail.ExpiryDate, paymentDetail.ExpiryDate);
            Assert.Equal(bankPaymentDetail.Amount, paymentDetail.Amount);
            Assert.Equal(bankPaymentDetail.Currency, paymentDetail.Currency);
            Assert.Equal(bankPaymentDetail.Cvv, paymentDetail.Cvv);
        }

        [Fact]
        public void Should_Mask_CardNumber_When_Mapping_BankPaymentDetail_To_PaymentDetail()
        {
            
            // Arrange
            var bankPaymentDetail = new BankPaymentDetail(Guid.NewGuid(), "Accepted", "4485063526474709", "01/20", 20.00m, "GBP", 123);
            var bankPaymentMapper = new BankPaymentMapper();

            // Act
            var paymentDetail = bankPaymentMapper.MapDetail(bankPaymentDetail);

            // Assert
            Assert.Equal("4***********4709", paymentDetail.CardNumber);
        }
    }
}