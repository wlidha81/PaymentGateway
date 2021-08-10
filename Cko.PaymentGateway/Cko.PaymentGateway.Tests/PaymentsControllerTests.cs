using Cko.PaymentGateway.Business;
using Cko.PaymentGateway.Business.Exceptions;
using Cko.PaymentGateway.Controllers;
using Cko.PaymentGateway.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Cko.PaymentGateway.Tests
{
    public class PaymentsControllerTests
    {
        [Fact]
        public async Task GetPaymentDetailsTest()
        {
            Mock<IPaymentService> paymentServiceMock = new Mock<IPaymentService>();
            ILogger<PaymentsController> logger = NullLoggerFactory.Instance.CreateLogger<PaymentsController>();


            var controller = new PaymentsController(paymentServiceMock.Object, logger);
            var result = await controller.GetPaymentDetails(string.Empty).ConfigureAwait(false);
            var objectResult = result as BadRequestObjectResult;
            Assert.NotNull(objectResult);
            var returnedDetailsDto =  new PaymentDetailsDto
            {
                CreditCardNumber = "12123132132132132",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
                PaymentId = "12112313132313121FE",
                PaymentStatus = "Paid"
            };

            paymentServiceMock.Setup(ps => ps.GetPaymentDetails(It.Is<string>(s => s == "12123132132132132"))).Returns(Task.FromResult(returnedDetailsDto));
            paymentServiceMock.Setup(ps => ps.GetPaymentDetails(It.Is<string>(s => s != "12123132132132132"))).Throws(new PaymentNotFoundException());
            var result1 = await controller.GetPaymentDetails("12123132132132132").ConfigureAwait(false);
            var objectResult1 = result1 as OkObjectResult;
            Assert.NotNull(objectResult1);
            var objectValue = objectResult1.Value as PaymentDetailsDto;
            Assert.NotNull(objectValue);
            Assert.Equal(returnedDetailsDto, objectValue);

            var result2 = await controller.GetPaymentDetails("12123132132132131").ConfigureAwait(false);
            var objectResult2 = result2 as NotFoundResult;
            Assert.NotNull(objectResult2);
        }

        [Fact]
        public async Task ProcessPaymentTest()
        {
            Mock<IPaymentService> paymentServiceMock = new Mock<IPaymentService>();
            ILogger<PaymentsController> logger = NullLoggerFactory.Instance.CreateLogger<PaymentsController>();

            var request = new PaymentRequestDto
            {
                CreditCardNumber = "12123132132132132",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
            };

            var invalidRequest = new PaymentRequestDto
            {
                CreditCardNumber = "12123132132132131",
                CreditCardExpiry = "11/22",
                CreditCardVerification = "121",
                PaymentAmount = 1121212.253M,
                PaymentCurrency = "EUR",
            };

            var paymentProcessingDto = new PaymentProcessingResultDto
            {
                PaymentId = "lkmlskdmskdms",
                PaymentStatus = "Paid"
            };

            paymentServiceMock.Setup(ps => ps.ProcessPayment(It.Is<PaymentRequestDto>(req => req.CreditCardNumber == "12123132132132132"))).Returns(Task.FromResult(paymentProcessingDto));
            paymentServiceMock.Setup(ps => ps.ProcessPayment(It.Is<PaymentRequestDto>(req => req.CreditCardNumber != "12123132132132132"))).Throws(new PaymentProcessingException());
            var controller = new PaymentsController(paymentServiceMock.Object, logger);
            var result = await controller.ProcessPayment(request).ConfigureAwait(false);
            var actionResult = result as CreatedAtActionResult;
            Assert.NotNull(actionResult);
            Assert.Equal(paymentProcessingDto, actionResult.Value);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);

            var result1 = await controller.ProcessPayment(invalidRequest).ConfigureAwait(false);
            var objectResult1 = result1 as ObjectResult;
            Assert.NotNull(objectResult1);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult1.StatusCode);
            Assert.IsType<string>(objectResult1.Value);
        }
    }
}
