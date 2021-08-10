using Cko.PaymentGateway.Data.Dtos;
using System;
using System.Threading.Tasks;

namespace Cko.PaymentGateway.Business
{
    /// <summary>
    /// This interface defines methods 
    /// that deal with Payment processing and payment details gathering.
    /// implementations should be able to process a <see cref="PaymentRequestDto"/>
    /// and return a <see cref="PaymentDetailsDto"/> given a payment id (guid)
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Process a Payment Request.
        /// and returns a <see cref="PaymentProcessingResultDto"/>
        /// that hodls the paymentId and payment procvessing result
        /// </summary>
        /// <param name="payment">the <see cref="PaymentRequestDto"/> to be processed</param>
        /// <returns>a <see cref="PaymentProcessingResultDto"/> that holds a payment identifier and a status</returns>
        Task<PaymentProcessingResultDto> ProcessPayment(PaymentRequestDto payment);

        /// <summary>
        /// This method should return a <see cref="PaymentDetailsDto"/>
        /// that holds informations related to a payment operation given its id (guid format)
        /// </summary>
        /// <param name="paymentId">the payment identifier</param>
        /// <returns>a <see cref="PaymentDetailsDto"/> that holds paymen</returns>
        Task<PaymentDetailsDto> GetPaymentDetails(string paymentId);
    }
}
