using System;
using System.Runtime.Serialization;

namespace Cko.PaymentGateway.Business.Exceptions
{
    [Serializable]
    public class PaymentProcessingException : Exception
    {
        public PaymentProcessingException()
        {
        }

        public PaymentProcessingException(string message) : base(message)
        {
        }

        public PaymentProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PaymentProcessingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}