using Newtonsoft.Json;
using System;

namespace Cko.PaymentGateway.Data.Dtos
{
    /// <summary>
    /// this class holds a DTO represent a payment request
    /// </summary>
    public class PaymentRequestDto
    {
        /// <summary>
        /// credit card number
        /// </summary>
        [JsonProperty("cardNumber")]
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// credit card expiry
        /// </summary>
        [JsonProperty("expiry")]
        public string CreditCardExpiry { get; set; }

        /// <summary>
        /// payment amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// payment currency
        /// </summary>
        [JsonProperty("currency")]
        public string PaymentCurrency { get; set; }

        /// <summary>
        /// credit card ccv
        /// </summary>
        [JsonProperty("ccv")]
        public string CreditCardVerification { get; set; }
    }
}
