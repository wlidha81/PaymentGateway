using Cko.PaymentGateway.Business;
using Cko.PaymentGateway.Business.Exceptions;
using Cko.PaymentGateway.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cko.PaymentGateway.Controllers
{
    /// <summary>
    /// this controller manages payment requests
    /// It allows to process and retrieve payment operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        private readonly ILogger<PaymentsController> _logger;
        /// <summary>
        /// Get Payment details given a payment Id
        /// </summary>
        /// <param name="id">the payment id</param>
        /// <returns>a <see cref="IActionResult"/></returns>
        // GET api/<PaymentsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaymentDetailsDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ResponseCache(Duration =60, VaryByQueryKeys =new[] {"id"})]
        public async Task<IActionResult> GetPaymentDetails(string id)
        {
            _logger.LogInformation($"Begin: loading payment details for payment with id:{id}]");
            if(string.IsNullOrEmpty(id))
            {
                _logger.LogError("Can not retrieve payment details for an invalid payment id.");
                return BadRequest("invalid payment id");
            }

            try
            {
                var result = await _paymentService.GetPaymentDetails(id).ConfigureAwait(false);
                return Ok(result);
            }
            catch(PaymentNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while retrieving payment with id {0}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }            
        }

        /// <summary>
        /// Process a payument request.
        /// if payment succeeds or fails, we should return an obect of type 
        /// <see cref="PaymentProcessingResultDto"/> holding the payment id and 
        /// its processing status
        /// </summary>
        /// <param name="paymentRequest">a <see cref="PaymentRequestDto"/> that holds the </param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(string.Format("invalid paymentRequest {0}", String.Join(Environment.NewLine, 
                    ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))));
            }

            try
            {
                var paymentResult = await _paymentService.ProcessPayment(paymentRequest).ConfigureAwait(false);

                return CreatedAtAction(nameof(GetPaymentDetails), new { id = paymentResult.PaymentId }, paymentResult);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
