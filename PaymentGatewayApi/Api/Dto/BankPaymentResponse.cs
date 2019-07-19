using System;

namespace PaymentGatewayApi.Api.Dto
{
    public class BankPaymentResponse
    {
        public Guid PaymentReference { get; }

        public string Status { get; }

        public BankPaymentResponse(Guid paymentReference, string status)
        {
            PaymentReference = paymentReference;
            this.Status = status;
        }
    }
}