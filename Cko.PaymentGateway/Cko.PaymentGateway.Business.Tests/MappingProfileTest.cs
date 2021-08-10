using AutoMapper;
using Cko.PaymentGateway.Business.Mappers;
using Cko.PaymentGateway.Data.Dtos;
using Cko.PaymentGateway.Data.Models;
using Xunit;

namespace Cko.PaymentGateway.Business.Tests
{
    public class MappingProfileTest
    {
        private IMapper _mapper;

        public MappingProfileTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void GivenAPaymentRequestDtoObject_WhenWeMapitToPaymentRequest_ThenMappedToAndMappedFromObjectsShouldShareSameCommonProperties()
        {
            var paymentRequest = TestObjectsBuilder.CreateADummyPaymentRequest();
            var paymentRequestDto = _mapper.Map<PaymentRequestDto>(paymentRequest);
            CheckMapping(paymentRequest, paymentRequestDto);
        }

        [Fact]
        public void GivenAPaymentRequestObject_WhenWeMapitToPaymentRequestDto_ThenMappedToAndMappedFromObjectsShouldShareSameCommonProperties()
        {
            var paymentRequestDto = TestObjectsBuilder.CreateADummyPaymentRequestDto();
            var paymentRequest = _mapper.Map<PaymentRequest>(paymentRequestDto);
            CheckMapping(paymentRequest, paymentRequestDto);
        }

        private static void CheckMapping(PaymentRequest paymentRequest, PaymentRequestDto paymentRequestDto)
        {
            Assert.NotNull(paymentRequest);
            Assert.NotNull(paymentRequestDto);

            Assert.Equal(paymentRequest.CreditCardNumber, paymentRequestDto.CreditCardNumber);
            Assert.Equal(paymentRequest.CreditCardExpiry, paymentRequestDto.CreditCardExpiry);
            Assert.Equal(paymentRequest.PaymentAmount, paymentRequestDto.PaymentAmount);
            Assert.Equal(paymentRequest.PaymentCurrency, paymentRequestDto.PaymentCurrency);
            Assert.Equal(paymentRequest.CreditCardVerification, paymentRequestDto.CreditCardVerification);
        }



        [Fact]
        public void GivenAPaymentDetailsDtoObject_WhenWeMapitToPaymentDetails_ThenMappedToAndMappedFromObjectsShouldShareSameCommonProperties()
        {
            var paymentDetailsDto = TestObjectsBuilder.CreateADummyPaymentDetailsDto();
            var paymentDetails = _mapper.Map<PaymentDetails>(paymentDetailsDto);
            CheckMapping(paymentDetails, paymentDetailsDto);
        }

        [Fact]
        public void GivenAPaymentDetailsObject_WhenWeMapitToPaymentDetailsDto_ThenMappedToAndMappedFromObjectsShouldShareSameCommonProperties()
        {
            var paymentDetails = TestObjectsBuilder.CreateADummyPaymentDetails();
            var paymentDetailsDto = _mapper.Map<PaymentDetailsDto>(paymentDetails);
            CheckMapping(paymentDetails, paymentDetailsDto);
        }

        private static void CheckMapping(PaymentDetails paymentDetails, PaymentDetailsDto paymentDetailsDto)
        {
            Assert.NotNull(paymentDetails);
            Assert.NotNull(paymentDetailsDto);

            Assert.Equal(paymentDetails.CreditCardNumber, paymentDetailsDto.CreditCardNumber);
            Assert.Equal(paymentDetails.CreditCardExpiry, paymentDetailsDto.CreditCardExpiry);
            Assert.Equal(paymentDetails.PaymentAmount, paymentDetailsDto.PaymentAmount);
            Assert.Equal(paymentDetails.PaymentCurrency, paymentDetailsDto.PaymentCurrency);
            Assert.Equal(paymentDetails.CreditCardVerification, paymentDetailsDto.CreditCardVerification);
            Assert.Equal(paymentDetails.PaymentId, paymentDetailsDto.PaymentId);
            Assert.Equal(paymentDetails.PaymentStatus.ToString(), paymentDetailsDto.PaymentStatus);
        }

        [Fact]
        public void GivenAPaymentProcessingResultDtoObject_WhenWeMapitToPaymentProcessingResult_ThenMappedToAndMappedFromObjectsShouldShareSameCommonProperties()
        {
            var paymentProcessingResultDto = TestObjectsBuilder.CreateADummyPaymentProcessingResultDto();
            var paymentProcessingResult = _mapper.Map<PaymentProcessingResult>(paymentProcessingResultDto);
            CheckMapping(paymentProcessingResult, paymentProcessingResultDto);
        }

        [Fact]
        public void GivenAPaymentProcessingResultObject_WhenWeMapitToPaymentProcessingResultDto_ThenMappedToAndMappedFromObjectsShouldShareSameCommonProperties()
        {
            var paymentProcessingResult = TestObjectsBuilder.CreateADummyPaymentProcessingResult();
            var paymentProcessingResultDto = _mapper.Map<PaymentProcessingResultDto>(paymentProcessingResult);
            CheckMapping(paymentProcessingResult, paymentProcessingResultDto);
        }

        private static void CheckMapping(PaymentProcessingResult paymentProcessingResult, PaymentProcessingResultDto paymentProcessingResultDto)
        {
            Assert.NotNull(paymentProcessingResult);
            Assert.NotNull(paymentProcessingResultDto);

            Assert.Equal(paymentProcessingResult.PaymentId, paymentProcessingResultDto.PaymentId);
            Assert.Equal(paymentProcessingResult.PaymentStatus.ToString(), paymentProcessingResultDto.PaymentStatus);
        }
    }
}
