using ChatKid.Application.Models.RequestModels.UserProfileRequests;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators.ProfileUserValidator
{
    public class UserProfileUpdateValidator : ExceptionValidator<UserProfileUpdateRequests>
    {
        public UserProfileUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }

    }
}
