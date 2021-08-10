using AutoMapper;
using Cko.PaymentGateway.Data.Models;
using Cko.PaymentGateway.Data.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cko.PaymentGateway.BankSimulator
{
    public class AcquiringBank : IAcquiringBank
    {
        private ConcurrentDictionary<string, PaymentDetails> _payments = new ConcurrentDictionary<string, PaymentDetails>();
        private readonly IValidator<PaymentRequest> _validator;
        private readonly ILogger<AcquiringBank> _logger;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IServiceProvider _serviceProvider;


        public AcquiringBank(IValidator<PaymentRequest> validator, ILogger<AcquiringBank> logger, IMapper mapper, IServiceProvider serviceProvider)
        {
            this._validator = validator;
            this._logger = logger;
            this._mapper = mapper;
            this._serviceProvider = serviceProvider;
            this._httpClient.BaseAddress = new Uri("https://www.google.com");
        }

        public Task<PaymentDetails> GetPayment(string paymentId)
        {
            return Task.FromResult(_payments.ContainsKey(paymentId) ? _payments[paymentId] : default);
        }

        public async Task<PaymentProcessingResult> PerformPayment(PaymentRequest paymentRequest)
        {
            var paymentId = Guid.NewGuid().ToString();
            var paymentDetails = _mapper.Map<PaymentDetails>(paymentRequest);
            paymentDetails.PaymentId = paymentId;
            paymentDetails.PaymentStatus = PaymentStatus.Paid;
            //using(var scope = _serviceProvider.CreateScope())
            {
                //IValidator<PaymentRequest> validator = scope.ServiceProvider.GetRequiredService<IValidator<PaymentRequest>>();
                //if (!validator.Validate(paymentRequest).IsValid)
                if (!_validator.Validate(paymentRequest).IsValid)
                {
                    paymentDetails.PaymentStatus = PaymentStatus.Refused;
                }
                //forward to 3rd party service (simulate a call to cgheckout.com)
                await _httpClient.GetAsync("search?q=checkout.com").ConfigureAwait(false);
                //save result
                _payments.TryAdd(paymentId, paymentDetails);

                return new PaymentProcessingResult
                {
                    PaymentId = paymentId,
                    PaymentStatus = paymentDetails.PaymentStatus
                };
            }
            
        }
    }
}