using Cko.PaymentGateway.Data.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cko.PaymentGateway.IntegrationTests
{
    public class PaymentsControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public PaymentsControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ProcessPaymentAndGetDetailsTest()
        {
            var client = _factory.CreateClient();

            var paymentRequestDto = new PaymentRequestDto
            {
                CreditCardNumber = "4212313213213213",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
            };

            var json = JsonConvert.SerializeObject(paymentRequestDto);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("http://localhost/api/Payments", stringContent).ConfigureAwait(false);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.Created);
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var processingResult = JsonConvert.DeserializeObject<PaymentProcessingResultDto>(content);
            Assert.NotNull(processingResult);
            Assert.Equal("Paid", processingResult.PaymentStatus);
            Assert.NotEmpty(processingResult.PaymentId);

            var paymentDetailsResult = await client.GetAsync($"http://localhost/api/Payments/{processingResult.PaymentId}").ConfigureAwait(false);
            Assert.Equal(System.Net.HttpStatusCode.OK, paymentDetailsResult.StatusCode);
            content = await paymentDetailsResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            var paymentDetailsDto = JsonConvert.DeserializeObject<PaymentDetailsDto>(content);
            Assert.NotNull(paymentDetailsDto);
            Assert.Equal("Paid", paymentDetailsDto.PaymentStatus);
            Assert.Equal(processingResult.PaymentId, paymentDetailsDto.PaymentId);
            Assert.Equal(paymentRequestDto.CreditCardNumber, paymentDetailsDto.CreditCardNumber);
            Assert.Equal(paymentRequestDto.CreditCardExpiry, paymentDetailsDto.CreditCardExpiry);
            Assert.Equal(paymentRequestDto.CreditCardVerification, paymentDetailsDto.CreditCardVerification);
            Assert.Equal(paymentRequestDto.PaymentAmount, paymentDetailsDto.PaymentAmount);
            Assert.Equal(paymentRequestDto.PaymentCurrency, paymentDetailsDto.PaymentCurrency);

            var notFoundResult = await client.GetAsync($"http://localhost/api/payments/1422").ConfigureAwait(false);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, notFoundResult.StatusCode);

            paymentRequestDto.PaymentCurrency = "AED";//not valid for payment
            json = JsonConvert.SerializeObject(paymentRequestDto);
            stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            result = await client.PostAsync("http://localhost/api/payments", stringContent).ConfigureAwait(false);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.Created);
            content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            paymentDetailsDto = JsonConvert.DeserializeObject<PaymentDetailsDto>(content);
            Assert.NotNull(paymentDetailsDto);
            Assert.Equal("Refused", paymentDetailsDto.PaymentStatus);
        }
    }
}
