using Newtonsoft.Json;

namespace Cko.PaymentGateway.Data.Dtos
{
    /// <summary>
    /// this Dto class holds a DTO that represents payment details 
    /// </summary>
    public class PaymentDetailsDto
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

        [JsonProperty("ccv")]
        public string CreditCardVerification { get; set; }

        [JsonProperty("status")]
        public string PaymentStatus { get; set; }

        [JsonProperty("id")]
        public string PaymentId { get; set; }
    }
}
