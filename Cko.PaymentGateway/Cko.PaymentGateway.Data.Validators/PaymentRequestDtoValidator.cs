using Cko.PaymentGateway.Data.Dtos;
using FluentValidation;
using System;

namespace Cko.PaymentGateway.Data.Validators
{
    /// <summary>
    /// This Validator class allows to validate a <see cref="PaymentRequestDto"/>
    /// </summary>
    public class PaymentRequestDtoValidator : AbstractValidator<PaymentRequestDto>
    {
        public PaymentRequestDtoValidator()
        {
            //credit card must not be empty and should match a regular exprfession pattern
            RuleFor(p => p.CreditCardNumber)
                .NotEmpty()
                .Length(16)
                .Matches("^4[0-9]{12}(?:[0-9]{3})?$");//example for visa card

            //credit card expiry shoud not be empty and should match regular expression MM/yyyy or MM/YY
            RuleFor(p => p.CreditCardExpiry)
                 .NotEmpty()
                .Length(5, 7)
                .Matches(@"^(0[1-9]|1[0-2])\/?(([0-9]{4}|[0-9]{2})$)");

            //the payment amount should be greater then 0
            RuleFor(p => p.PaymentAmount)
                .GreaterThan(0);

            // the payment currency should not be empty and should match a regular expression
            RuleFor(p => p.PaymentCurrency)
                .NotEmpty()
                .Length(3)
                .Matches("[A-Z]{3}");
            
            //credit card verification should not be empty and should match a regular expression
            RuleFor(p => p.CreditCardVerification)
                .NotEmpty()
                .Length(3)
                .Matches("[0-9]{3}");
        }
    }
}
