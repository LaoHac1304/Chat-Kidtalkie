using ChatKid.Application.Models;
using ChatKid.Application.Models.RequestModels;
using ChatKid.Common.Validation;
using FluentValidation;

namespace KMS.Healthcare.TalentInventorySystem.Validators
{
    public class UserLoginValidator : ExceptionValidator<LoginRequest>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty();
        }
    }
}
