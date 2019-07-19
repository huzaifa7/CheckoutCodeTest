using PaymentGatewayApi.Api.Dto;
using PaymentGatewayApi.Model;

namespace PaymentGatewayApi.Mapper
{
    public interface IBankPaymentMapper
    {
        BankPaymentRequest MapRequest(PaymentRequest paymentRequest);
        PaymentResponse MapResponse(BankPaymentResponse bankPaymentResponse);
        PaymentDetail MapDetail(BankPaymentDetail bankPaymentDetail);
    }
}