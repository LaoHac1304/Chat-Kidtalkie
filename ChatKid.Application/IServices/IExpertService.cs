using ChatKid.Application.Models.RequestModels.ExpertRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ExpertViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IExpertService
    {
        Task<ExpertViewModel> GetExpertByIdAsync(Guid? id);
        Task<ExpertViewModel> GetExpertByEmailAsync(string mail);
        Task<ExpertViewModel> GetExpertInfo(string? mail, string? avatar);
        Task<(int, List<ExpertViewModel>)> GetExpertPagesAsync(FilterViewModel filterViewModel, int pageIndex, int pageSize, string? sort);
        Task<CommandResult> AddExpertAsync(ExpertCreateRequest expertViewModel);
        Task<CommandResult> DeleteExpertAsync(Guid id);
        Task<CommandResult> UpdateExpertAsync(Guid id, ExpertUpdateRequest expertViewModel);

    }
}
