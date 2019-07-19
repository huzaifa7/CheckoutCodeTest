using System;

namespace PaymentGatewayApi.Model
{
    public class PaymentResponse
    {
        public Guid PaymentReference { get; }
        public string Status { get; }

        public PaymentResponse(Guid paymentReference, string status)
        {
            PaymentReference = paymentReference;
            Status = status;
        }
    }
}