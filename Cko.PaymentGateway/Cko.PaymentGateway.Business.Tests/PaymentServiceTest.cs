using AutoMapper;
using Cko.PaymentGateway.BankSimulator;
using Cko.PaymentGateway.Business.Exceptions;
using Cko.PaymentGateway.Business.Services.Implementations;
using Cko.PaymentGateway.Data.Dtos;
using Cko.PaymentGateway.Data.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cko.PaymentGateway.Business.Tests
{
    public class PaymentServiceTest
    {
        Mock<IAcquiringBank> _acuiqringBank = new Mock<IAcquiringBank>();
        ILogger<PaymentService> _logger = NullLoggerFactory.Instance.CreateLogger<PaymentService>();
        Mock<IMapper> _mapper = new Mock<IMapper>();
        Mock<IValidator<PaymentRequestDto>> _validator = new Mock<IValidator<PaymentRequestDto>>();
        
        [Fact]
        public async Task GetPaymentDetailsTest()
        {
            var paymentDetails = TestObjectsBuilder.CreateADummyPaymentDetails();
            var paymentDetailsDto = TestObjectsBuilder.CreateADummyPaymentDetailsDto();
            var paymentId = "12112313132313121FE";
            var notFoundPaymentId = "12112313132313121FA";
            _acuiqringBank.Setup(bank => bank.GetPayment(It.Is<string>(s => s == paymentId))).Returns(Task.FromResult(paymentDetails));
            _acuiqringBank.Setup(bank => bank.GetPayment(It.Is<string>(s => s != paymentId))).Returns(Task.FromResult((PaymentDetails)null));
            _mapper.Setup(map => map.Map<PaymentDetailsDto>(It.Is<PaymentDetails>(det => det.PaymentId == paymentId))).Returns(paymentDetailsDto);

            var service = new PaymentService(_acuiqringBank.Object, _logger, _mapper.Object, _validator.Object);
            var result = await service.GetPaymentDetails(paymentId).ConfigureAwait(false);
            _acuiqringBank.Verify(bank => bank.GetPayment(It.IsAny<string>()), Times.Once);
            _mapper.Verify(map => map.Map<PaymentDetailsDto>(It.Is<PaymentDetails>(det => det.PaymentId == paymentId)), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(paymentDetailsDto, result);

            var action = service.GetPaymentDetails(notFoundPaymentId);            
            await Assert.ThrowsAsync<PaymentNotFoundException>(() => action).ConfigureAwait(false);
            _acuiqringBank.Verify(bank => bank.GetPayment(It.IsAny<string>()), Times.Exactly(2));
            _mapper.Verify(map => map.Map<PaymentDetailsDto>(It.Is<PaymentDetails>(det => det.PaymentId == notFoundPaymentId)), Times.Never);
        }

        [Fact]
        public async Task ProcessPaymentTest()
        {
            var processingResult = TestObjectsBuilder.CreateADummyPaymentProcessingResult();
            var processingResultDto = TestObjectsBuilder.CreateADummyPaymentProcessingResultDto();
            var paymentRequestDto = TestObjectsBuilder.CreateADummyPaymentRequestDto();
            var paymentRequest = TestObjectsBuilder.CreateADummyPaymentRequest();
            var paymentRequest2 = TestObjectsBuilder.CreateADummyPaymentRequest();
            var paymentDetails = TestObjectsBuilder.CreateADummyPaymentDetails();
            var creditCardNumber = paymentRequestDto.CreditCardNumber;
            var erroredCreditCardNumber = creditCardNumber + "3";
            var validResult = new Mock<ValidationResult>();
            validResult.Setup(vr => vr.IsValid).Returns(true);
            var invalidResult = new Mock<ValidationResult>();
            invalidResult.Setup(vr => vr.IsValid).Returns(false);
            _acuiqringBank.Setup(bank => bank.PerformPayment(It.Is<PaymentRequest>(pr => pr.CreditCardNumber == creditCardNumber))).Returns(Task.FromResult(processingResult));
            _acuiqringBank.Setup(bank => bank.PerformPayment(It.Is<PaymentRequest>(pr => pr.CreditCardNumber != creditCardNumber))).Returns(Task.FromResult((PaymentProcessingResult)null));
            _mapper.Setup(map => map.Map<PaymentRequest>(It.Is<PaymentRequestDto>(det => det.CreditCardNumber == creditCardNumber))).Returns(paymentRequest);
            _mapper.Setup(map => map.Map<PaymentProcessingResultDto>(It.Is<PaymentProcessingResult>(det => det.PaymentId == processingResult.PaymentId))).Returns(processingResultDto);
            _validator.Setup(val => val.Validate(It.Is<PaymentRequestDto>(det => det.CreditCardNumber == creditCardNumber))).Returns(validResult.Object);
            _validator.Setup(val => val.Validate(It.Is<PaymentRequestDto>(det => det.CreditCardNumber != creditCardNumber))).Returns(invalidResult.Object);

            var service = new PaymentService(_acuiqringBank.Object, _logger, _mapper.Object, _validator.Object);
            var result = await service.ProcessPayment(paymentRequestDto).ConfigureAwait(false);
            _acuiqringBank.Verify(bank => bank.PerformPayment(It.Is<PaymentRequest>(pr => pr.CreditCardNumber == creditCardNumber)), Times.Once);
            _validator.Verify(val => val.ValidateAsync(It.Is<PaymentRequestDto>(det => det.CreditCardNumber == creditCardNumber), It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(map => map.Map<PaymentRequest>(It.Is<PaymentRequestDto>(det => det.CreditCardNumber == creditCardNumber)), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(processingResultDto, result);

            paymentRequestDto.CreditCardNumber = erroredCreditCardNumber;
            paymentRequest2.CreditCardNumber = paymentRequestDto.CreditCardNumber;
            _mapper.Setup(map => map.Map<PaymentRequest>(It.Is<PaymentRequestDto>(det => det.CreditCardNumber == paymentRequestDto.CreditCardNumber))).Returns(paymentRequest2);
            var action = service.ProcessPayment(paymentRequestDto);
            await Assert.ThrowsAsync<PaymentProcessingException>(() => action).ConfigureAwait(false);
            _acuiqringBank.Verify(bank => bank.PerformPayment(It.Is<PaymentRequest>(pr => pr.CreditCardNumber == creditCardNumber)), Times.Once);
            _validator.Verify(val => val.Validate(It.Is<PaymentRequestDto>(det => det.CreditCardNumber != creditCardNumber)), Times.Never);
            _validator.Verify(val => val.ValidateAsync(It.Is<PaymentRequestDto>(det => det.CreditCardNumber == creditCardNumber), It.IsAny<CancellationToken>()), Times.Never);
            _validator.Verify(val => val.ValidateAsync(It.Is<PaymentRequestDto>(det => det.CreditCardNumber != creditCardNumber), It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(map => map.Map<PaymentRequest>(It.Is<PaymentRequestDto>(det => det.CreditCardNumber != creditCardNumber)), Times.Exactly(2));
        }
    }
}
