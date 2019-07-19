using System.Text;
using PaymentGatewayApi.Api.Dto;
using PaymentGatewayApi.Model;

namespace PaymentGatewayApi.Mapper
{
    public class BankPaymentMapper : IBankPaymentMapper
    {
        public BankPaymentRequest MapRequest(PaymentRequest paymentRequest)
        {
            return new BankPaymentRequest(paymentRequest.CardNumber, paymentRequest.ExpiryDate, paymentRequest.Amount,
                paymentRequest.Currency, paymentRequest.Cvv);
        }

        public PaymentResponse MapResponse(BankPaymentResponse bankPaymentResponse)
        {
            return new PaymentResponse(bankPaymentResponse.PaymentReference, bankPaymentResponse.Status);
        }

        public PaymentDetail MapDetail(BankPaymentDetail bankPaymentDetail)
        {
            var cardNumber = MaskCardNumber(bankPaymentDetail.CardNumber);
            return new PaymentDetail(bankPaymentDetail.PaymentReference, bankPaymentDetail.Status,
                cardNumber, bankPaymentDetail.ExpiryDate, bankPaymentDetail.Amount,
                bankPaymentDetail.Currency, bankPaymentDetail.Cvv);
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