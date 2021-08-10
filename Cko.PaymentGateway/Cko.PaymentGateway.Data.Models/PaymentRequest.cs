using System;

namespace Cko.PaymentGateway.Data.Models
{
    /// <summary>
    /// This class represents a Payment request
    /// </summary>
    public class PaymentRequest
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
        /// payment card ccv
        /// </summary>
        public string CreditCardVerification { get; set; }
    }
}
