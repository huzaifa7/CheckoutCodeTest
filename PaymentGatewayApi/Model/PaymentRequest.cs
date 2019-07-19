using System;

namespace PaymentGatewayApi.Model
{
    public class PaymentRequest
    {
        public string CardNumber { get; }
        public string ExpiryDate { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public int Cvv { get; }

        public PaymentRequest(string cardNumber, string expiryDate, decimal amount, string currency, int cvv)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Amount = amount;
            Currency = currency;
            Cvv = cvv;
        }

        public bool Validate()
        {
            return !(string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(ExpiryDate) || Amount <= 0 ||
                     string.IsNullOrWhiteSpace(Currency) || Cvv <= 0);
        }
    }
}