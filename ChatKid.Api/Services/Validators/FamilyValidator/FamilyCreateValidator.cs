using ChatKid.Application.Models.RequestModels;
using ChatKid.Application.Models.RequestModels.FamilyRequests;
using ChatKid.Common.Validation;
using FluentValidation;

namespace ChatKid.Api.Services.Validators.FamilyValidator
{
    public class FamilyCreateValidator : ExceptionValidator<FamilyCreateRequest>
    {
        public FamilyCreateValidator() {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
