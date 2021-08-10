using Cko.PaymentGateway.Data.Models;
using System.Threading.Tasks;

namespace Cko.PaymentGateway.BankSimulator
{
    /// <summary>
    /// This interface holds contract methods for a bank simulator
    /// we use it in order to perform a payment or retrieve payment details
    /// </summary>
    public interface IAcquiringBank
    {
        Task<PaymentProcessingResult> PerformPayment(PaymentRequest paymentRequest);
        Task<PaymentDetails> GetPayment(string paymentId);
    }
}