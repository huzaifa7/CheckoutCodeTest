using System;
using System.Threading.Tasks;
using PaymentGatewayApi.Api.Dto;

namespace PaymentGatewayApi.Api
{
    public interface IBankApiClient
    {
        Task<BankPaymentResponse> Process(BankPaymentRequest paymentRequest);
        Task<BankPaymentDetail> GetPaymentDetail(Guid paymentReference);
    }
}