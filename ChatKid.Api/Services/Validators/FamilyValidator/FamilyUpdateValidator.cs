using ChatKid.Application.Models.RequestModels.FamilyRequests;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators.FamilyValidator
{
    public class FamilyUpdateValidator : ExceptionValidator<FamilyUpdateRequest>
    {
        public FamilyUpdateValidator() {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
