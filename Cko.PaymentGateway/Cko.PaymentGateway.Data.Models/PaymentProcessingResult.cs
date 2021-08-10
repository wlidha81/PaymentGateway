namespace Cko.PaymentGateway.Data.Models
{
    /// <summary>
    /// This class represents a payment processing result
    /// it holds a paymentId, and a PaymentStatus(Sucess or Failure)
    /// </summary>
    public class PaymentProcessingResult
    {
        /// <summary>
        /// the payment Id
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// the payment status.
        /// Possible values <see cref="PaymentStatus"/>        
        /// /// </summary>
        public PaymentStatus PaymentStatus { get; set; }
    }
}
