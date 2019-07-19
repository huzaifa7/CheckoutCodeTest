using System;

namespace BankApiStub.Dto
{
    public class BankPaymentDetail
    {
        public Guid PaymentReference { get; }
        public string Status { get; }
        public string CardNumber { get; }
        public string ExpiryDate { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public int Cvv { get; }

        public BankPaymentDetail(Guid paymentReference, string status, string cardNumber, string expiryDate, decimal amount, string currency, int cvv)
        {
            PaymentReference = paymentReference;
            Status = status;
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Amount = amount;
            Currency = currency;
            Cvv = cvv;
        }
    }
}