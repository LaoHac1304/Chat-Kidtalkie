using ChatKid.Application.Models.RequestModels;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators
{
    public class TokenValidator : ExceptionValidator<TokenRequest>
    {
        public TokenValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty();
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}
