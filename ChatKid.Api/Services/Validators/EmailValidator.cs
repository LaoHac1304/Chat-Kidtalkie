using ChatKid.Application.Models.RequestModels;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators
{
    public class EmailValidator : ExceptionValidator<EmailRequest>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is invalid!!! PLease following the format example@abc.com")
                .NotEmpty();
        }
    }
}
