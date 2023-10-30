using ChatKid.Application.Models.RequestModels;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators
{
    public class RegisterValidator : ExceptionValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty();
        }
    }
}
