using System;
using System.Text;
using System.Threading.Tasks;
using PaymentGatewayApi.Api;
using PaymentGatewayApi.Api.Dto;
using PaymentGatewayApi.Logging;
using PaymentGatewayApi.Mapper;
using PaymentGatewayApi.Model;

namespace PaymentGatewayApi.Service
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IBankApiClient _bankApiClient;
        private readonly IBankPaymentMapper _bankPaymentMapper;
        private readonly IApplicationLogger _applicationLogger;

        public PaymentGatewayService(IBankApiClient bankApiClient, IBankPaymentMapper bankPaymentMapper, IApplicationLogger applicationLogger)
        {
            _bankApiClient = bankApiClient;
            _bankPaymentMapper = bankPaymentMapper;
            _applicationLogger = applicationLogger;
        }

        public async Task<PaymentResponse> SendPayment(PaymentRequest paymentRequest)
        {
            //TODO: validate paymentRequest against business rules
            var bankPaymentRequest = _bankPaymentMapper.MapRequest(paymentRequest);

            var bankPaymentResponse = await _bankApiClient.Process(bankPaymentRequest);
            
            var paymentResponse = _bankPaymentMapper.MapResponse(bankPaymentResponse);
            _applicationLogger.LogInformation($"Finished processing payment request: {bankPaymentResponse.PaymentReference}");
            return paymentResponse;
        }

        public async Task<PaymentDetail> GetPaymentDetail(Guid paymentReference)
        {
            var bankPaymentDetail  = await _bankApiClient.GetPaymentDetail(paymentReference);
            //TODO: use cache-aside pattern to retrieve payment details from storage to improve performance and save cost.

            var paymentDetail = _bankPaymentMapper.MapDetail(bankPaymentDetail);
            
            //TODO: Enrich logger to capture context (i.e correlation id)
            _applicationLogger.LogInformation($"Finished handling request for payment reference: {paymentReference}");

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
    }
}