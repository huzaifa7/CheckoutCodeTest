using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGatewayApi.Api;
using PaymentGatewayApi.Api.Dto;
using PaymentGatewayApi.Logging;
using PaymentGatewayApi.Model;

namespace PaymentGatewayApi.Service
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IBankApiClient _bankApiClient;
        private readonly IApplicationLogger _applicationLogger;

        public PaymentGatewayService(IBankApiClient bankApiClient, IApplicationLogger applicationLogger)
        {
            _bankApiClient = bankApiClient;
            _applicationLogger = applicationLogger;
        }

        public async Task<PaymentResponse> SendPayment(PaymentRequest paymentRequest)
        {
            var bankPaymentRequest = CreateBankPaymentRequest(paymentRequest);

            var bankPaymentResponse = await _bankApiClient.Process(bankPaymentRequest);
            
            var paymentResponse = CreatePaymentResponse(bankPaymentResponse);
            _applicationLogger.LogInformation($"Finished processing payment request: {bankPaymentResponse.PaymentReference}");
            return paymentResponse;
        }

        public async Task<PaymentDetail> GetPaymentDetail(Guid paymentReference)
        {
            var bankPaymentDetail  = await _bankApiClient.GetPaymentDetail(paymentReference);
            //TODO: use cache-aside pattern to retrieve payment details from storage to improve performance and save cost.
            var maskCardNumber = MaskCardNumber(bankPaymentDetail.CardNumber);
            
            //TODO: Enriich logger to capture context (i.e correlation id)
            _applicationLogger.LogInformation($"Finished handling request for payment reference: {paymentReference}");

            var paymentDetail = new PaymentDetail(bankPaymentDetail.PaymentReference, bankPaymentDetail.Status, maskCardNumber,
                bankPaymentDetail.ExpiryDate, bankPaymentDetail.Amount, bankPaymentDetail.Currency,
                bankPaymentDetail.Cvv);

            return paymentDetail;
        }

        private static BankPaymentRequest CreateBankPaymentRequest(PaymentRequest paymentRequest)
        {
            return new BankPaymentRequest(paymentRequest.CardNumber, paymentRequest.ExpiryDate,
                paymentRequest.Amount, paymentRequest.Currency, paymentRequest.Cvv);
        }

        private static PaymentResponse CreatePaymentResponse(BankPaymentResponse bankPaymentResponse)
        {
            return new PaymentResponse(bankPaymentResponse.PaymentReference, bankPaymentResponse.Status);
        }

        private string MaskCardNumber(string cardNumber)
        {
            var maskedCardNumberBuilder = new StringBuilder(cardNumber.Substring(0,1));
            for (int i = 1; i < cardNumber.Length - 4; i++)
            {
                maskedCardNumberBuilder.Append("*");
            }

            maskedCardNumberBuilder.Append(cardNumber.Substring(cardNumber.Length - 4));

            return maskedCardNumberBuilder.ToString();
        }
    }
}