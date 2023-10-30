using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.SubcriptionViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface ISubcriptionService
    {
        public Task<SubcriptionViewModel> GetByIdAsync(Guid id);
        public Task<List<SubcriptionViewModel>> GetAllAsync(string searchString);
        public Task<(int, List<SubcriptionViewModel>)> GetPagesAsync(FilterViewModel filter, string? sortBy, int pageIndex, int pageSize);
        public Task<CommandResult> CreateAsync(SubcriptionViewModel model);
        public Task<CommandResult> UpdateAsync(SubcriptionViewModel model);
        public Task<CommandResult> DeleteAsync(Guid id);
    }
}
