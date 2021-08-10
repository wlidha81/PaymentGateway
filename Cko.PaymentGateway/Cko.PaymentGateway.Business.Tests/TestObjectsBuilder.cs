using Cko.PaymentGateway.Data.Dtos;
using Cko.PaymentGateway.Data.Models;

namespace Cko.PaymentGateway.Business.Tests
{
    internal static class TestObjectsBuilder
    {
        internal static PaymentDetailsDto CreateADummyPaymentDetailsDto()
        {
            return new PaymentDetailsDto
            {
                CreditCardNumber = "12123132132132132",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
                PaymentId = "12112313132313121FE",
                PaymentStatus = "Paid"
            };
        }

        internal static PaymentDetails CreateADummyPaymentDetails()
        {
            return new PaymentDetails
            {
                CreditCardNumber = "12123132132132132",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
                PaymentId = "12112313132313121FE",
                PaymentStatus = PaymentStatus.Refused
            };
        }

        internal static PaymentProcessingResult CreateADummyPaymentProcessingResult()
        {
            return new PaymentProcessingResult
            {
                PaymentId = "12112313132313121FE",
                PaymentStatus = PaymentStatus.Refused
            };
        }

        internal static PaymentProcessingResultDto CreateADummyPaymentProcessingResultDto()
        {
            return new PaymentProcessingResultDto
            {
                PaymentId = "12112313132313121FE",
                PaymentStatus = PaymentStatus.Paid.ToString()
            };
        }

        internal static PaymentRequestDto CreateADummyPaymentRequestDto()
        {
            return new PaymentRequestDto
            {
                CreditCardNumber = "12123132132132132",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
            };
        }


        internal static PaymentRequest CreateADummyPaymentRequest()
        {
            return new PaymentRequest
            {
                CreditCardNumber = "12123132132132132",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
            };
        }
    }
}
