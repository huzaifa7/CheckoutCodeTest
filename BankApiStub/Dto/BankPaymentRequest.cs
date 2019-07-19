namespace BankApiStub.Dto
{
    public class BankPaymentRequest
    {
        public string CardNumber { get; }
        public string ExpiryDate { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public int Cvv { get; }

        public BankPaymentRequest(string cardNumber, string expiryDate, decimal amount, string currency, int cvv)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Amount = amount;
            Currency = currency;
            Cvv = cvv;
        }
    }
}