using Cko.PaymentGateway.Data.Models;
using Xunit;

namespace Cko.PaymentGateway.Data.Validators.Tests
{
    public class PaymentRequestValidatorTest
    {
        private PaymentRequest _paymentRequest;
        private PaymentRequestValidator _validator;
        public PaymentRequestValidatorTest()
        {
            Init();
        }

        private void Init()
        {
            _paymentRequest = new PaymentRequest
            {
                CreditCardNumber = "4234567891234567",
                CreditCardExpiry = "01/22",
                CreditCardVerification = "111",
                PaymentAmount = 1213131.2536M,
                PaymentCurrency = "EUR"
            };
            _validator = new PaymentRequestValidator();
        }

        [Theory]
        [InlineData("42345678912345678", false)]
        [InlineData("42345678912-45678", false)]
        [InlineData("12345678912-45678", false)]
        [InlineData("4234567891234567", true)]
        public void ValidateCreditCardNumberTest(string cardNumber, bool expectedIsValid)
        {
            Init();
            _paymentRequest.CreditCardNumber = cardNumber;
            var validationResult = _validator.Validate(_paymentRequest);
            Assert.Equal(expectedIsValid, validationResult.IsValid);
            if (expectedIsValid)
            {
                Assert.Empty(validationResult.Errors);
            }
            else
            {
                Assert.Equal(2, validationResult.Errors.Count);
                validationResult.Errors.Exists(err => err.PropertyName == nameof(_paymentRequest.CreditCardNumber));
            }

        }

        [Theory]
        [InlineData("232/11", false)]
        [InlineData("A1/11", false)]
        [InlineData("01/2021", true)]
        [InlineData("02/2021", true)]
        [InlineData("03/2021", true)]
        [InlineData("04/2021", true)]
        [InlineData("05/2021", true)]
        [InlineData("06/2021", true)]
        [InlineData("07/2021", true)]
        [InlineData("08/2021", true)]
        [InlineData("09/2021", true)]
        [InlineData("10/2021", true)]
        [InlineData("11/2021", true)]
        [InlineData("12/2021", true)]
        [InlineData("01/21", true)]
        [InlineData("02/21", true)]
        [InlineData("03/21", true)]
        [InlineData("04/21", true)]
        [InlineData("05/21", true)]
        [InlineData("06/21", true)]
        [InlineData("07/21", true)]
        [InlineData("08/21", true)]
        [InlineData("09/21", true)]
        [InlineData("10/21", true)]
        [InlineData("11/21", true)]
        [InlineData("12/21", true)]
        [InlineData("13/21", false)]
        public void ValidateCreditCardExpiryTest(string expiry, bool expectedIsValid)
        {
            Init();
            _paymentRequest.CreditCardExpiry = expiry;
            var validationResult = _validator.Validate(_paymentRequest);
            Assert.Equal(expectedIsValid, validationResult.IsValid);
            if (expectedIsValid)
            {
                Assert.Empty(validationResult.Errors);
            }
            else
            {
                Assert.Single(validationResult.Errors);
                validationResult.Errors.Exists(err => err.PropertyName == nameof(_paymentRequest.CreditCardExpiry));
            }

        }

        [Theory]
        [InlineData("A11a", false)]
        [InlineData("A11", false)]
        [InlineData("1234", false)]
        [InlineData("124", true)]
        public void ValidateCreditCardVerificationCodeTest(string ccv, bool expectedIsValid)
        {
            Init();
            _paymentRequest.CreditCardVerification = ccv;
            var validationResult = _validator.Validate(_paymentRequest);
            Assert.Equal(expectedIsValid, validationResult.IsValid);
            if (expectedIsValid)
            {
                Assert.Empty(validationResult.Errors);
            }
            else
            {
                Assert.NotEmpty(validationResult.Errors);
                Assert.All(validationResult.Errors, err => Assert.True(err.PropertyName == nameof(_paymentRequest.CreditCardVerification)));
            }

        }

        [Theory]
        [InlineData(-1212.25, false)]
        [InlineData(122.36, true)]
        [InlineData(0, false)]
        public void ValidatePaymentAmountTest(decimal amount, bool expectedIsValid)
        {
            Init();
            _paymentRequest.PaymentAmount = amount;
            var validationResult = _validator.Validate(_paymentRequest);
            Assert.Equal(expectedIsValid, validationResult.IsValid);
            if (expectedIsValid)
            {
                Assert.Empty(validationResult.Errors);
            }
            else
            {
                Assert.Single(validationResult.Errors);
                validationResult.Errors.Exists(err => err.PropertyName == nameof(_paymentRequest.PaymentAmount));
            }

        }

        [Theory]
        [InlineData("add", false)]
        [InlineData("eur", false)]
        [InlineData("EUR", true)]
        [InlineData("AED", false)]
        public void ValidatePaymentCurrencyTest(string currency, bool expectedIsValid)
        {
            Init();
            _paymentRequest.PaymentCurrency = currency;
            var validationResult = _validator.Validate(_paymentRequest);
            Assert.Equal(expectedIsValid, validationResult.IsValid);
            if (expectedIsValid)
            {
                Assert.Empty(validationResult.Errors);
            }
            else
            {
                Assert.NotEmpty(validationResult.Errors);
                Assert.All(validationResult.Errors, err => Assert.True(err.PropertyName == nameof(_paymentRequest.PaymentCurrency)));
            }
        }
    }
}
