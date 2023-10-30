using ChatKid.Application.Models.RequestModels.QuestionRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IQuestionService
    {
        Task <QuestionViewModel> GetQuestionByIdAsync(Guid id);
        Task <(int, IEnumerable<QuestionViewModel>)> GetQuestionPagesAsync(FilterViewModel filterViewModel, int pageIndex, int pageSize, string? sort);
        Task <CommandResult> CreateQuestionAsync(QuestionCreateRequest questionViewModel);
        Task <CommandResult> UpdateQuestionAsync(Guid id, QuestionUpdateRequest questionViewModel);
        Task <CommandResult> DeleteQuestionAsync(Guid id);
    }
}
