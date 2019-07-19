using System;
using System.Threading.Tasks;
using PaymentGatewayApi.Model;

namespace PaymentGatewayApi.Service
{
    public interface IPaymentGatewayService
    {
        Task<PaymentResponse> SendPayment(PaymentRequest paymentRequest);

        Task<PaymentDetail> GetPaymentDetail(Guid paymentReference);
    }
}