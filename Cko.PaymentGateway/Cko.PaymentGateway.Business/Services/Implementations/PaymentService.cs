using AutoMapper;
using Cko.PaymentGateway.BankSimulator;
using Cko.PaymentGateway.Business.Exceptions;
using Cko.PaymentGateway.Data.Dtos;
using Cko.PaymentGateway.Data.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cko.PaymentGateway.Business.Services.Implementations
{
    /// <summary>
    /// Concrete implemtation of the <see cref="IPaymentService"/>
    /// it will process payment through cko bank simulator.
    /// 
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IAcquiringBank _acquiringBank;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<PaymentRequestDto> _validator;
        public PaymentService(IAcquiringBank acquiringBank, ILogger<PaymentService> logger, IMapper mapper, IValidator<PaymentRequestDto> validator)
        {
            _acquiringBank = acquiringBank;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }
        /// <summary>
        /// fetch payment details given a payment identifier
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public async Task<PaymentDetailsDto> GetPaymentDetails(string paymentId)
        {
            _logger.LogInformation("Begin: GetPaymentDetails for payment with id:{0}", paymentId);
            var payment = await _acquiringBank.GetPayment(paymentId).ConfigureAwait(false);

            if (payment == null)
            {
                _logger.LogError("No PaymentDetails was found matching payment id:{0}", paymentId);
                throw new PaymentNotFoundException("payment with id{0} was not found");
            }
            _logger.LogInformation("A payment details with id:{0} was found", paymentId);
            return _mapper.Map<PaymentDetailsDto>(payment);
        }

        /// <summary>
        /// Process a payment request via cko bank simulator
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public async Task<PaymentProcessingResultDto> ProcessPayment(PaymentRequestDto payment)
        {
            await _validator.ValidateAndThrowAsync(payment);
            var paymentRequest = _mapper.Map<PaymentRequest>(payment);
            try
            {
                var paymentResult = await _acquiringBank.PerformPayment(paymentRequest).ConfigureAwait(false);
                if (paymentResult != null)
                {
                    return _mapper.Map<PaymentProcessingResultDto>(paymentResult);
                }
            }
            catch (Exception ex)
            {
                throw new PaymentProcessingException("An error occured while trying to process the payment", ex);
            }
         
            throw new PaymentProcessingException("Could not perform payment.");
        }
    }
}
