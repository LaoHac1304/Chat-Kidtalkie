using ChatKid.Application.Models.RequestModels.UserProfileRequests;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators.ProfileUserValidator
{
    public class UserProfileCreateValidator : ExceptionValidator<UserProfileCreateRequests>
    {
        public UserProfileCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.FamilyId)
                .NotEmpty();
        }
    }
}
