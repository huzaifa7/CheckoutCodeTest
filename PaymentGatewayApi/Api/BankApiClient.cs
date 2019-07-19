using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentGatewayApi.Api.Dto;

namespace PaymentGatewayApi.Api
{
    public class BankApiClient : IBankApiClient
    {
        private readonly HttpClient _httpClient;

        public BankApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<BankPaymentResponse> Process(BankPaymentRequest paymentRequest)
        {
            var serialisedRequest = JsonConvert.SerializeObject(paymentRequest);
            var httpResponse = await _httpClient.PostAsync("api/stub", new StringContent(serialisedRequest, Encoding.UTF8, "application/json"));
            var bankResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BankPaymentResponse>(bankResponse);
        }

        public async Task<BankPaymentDetail> GetPaymentDetail(Guid paymentReference)
        {
            var httpResponse = await _httpClient.GetAsync($"api/stub/{paymentReference}");
            var bankResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BankPaymentDetail>(bankResponse);
        }
    }
}