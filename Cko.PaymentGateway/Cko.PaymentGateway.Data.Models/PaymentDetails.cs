
namespace Cko.PaymentGateway.Data.Models
{
    /// <summary>
    /// This class represent a PaymentDetails
    /// </summary>
    public class PaymentDetails
    {
        /// <summary>
        /// credit card number
        /// </summary>
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// credit card expiry
        /// </summary>
        public string CreditCardExpiry { get; set; }

        /// <summary>
        /// payment amount
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// payment currency
        /// </summary>
        public string PaymentCurrency { get; set; }

        /// <summary>
        /// credit card ccv
        /// </summary>
        public string CreditCardVerification { get; set; }

        /// <summary>
        /// the payment processing status
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// the payment identifier
        /// </summary>
        public string PaymentId { get; set; }
    }
}
