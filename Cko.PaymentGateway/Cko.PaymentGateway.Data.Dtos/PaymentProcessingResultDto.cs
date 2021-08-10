using Newtonsoft.Json;

namespace Cko.PaymentGateway.Data.Dtos
{
    /// <summary>
    /// this class holds a DTO represent a payment processing result
    /// </summary>
    public class PaymentProcessingResultDto
    {
        /// <summary>
        /// the payment identifier
        /// </summary>
        [JsonProperty("id")]
        public string PaymentId { get; set; }

        /// <summary>
        /// the payment status(Sucess or Failure)
        /// </summary>
        [JsonProperty("status")]
        public string PaymentStatus { get; set; }
    }
}
