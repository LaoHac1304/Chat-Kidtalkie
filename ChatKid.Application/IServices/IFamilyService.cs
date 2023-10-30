using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.FamilyViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IFamilyService
    {
        public Task<FamilyViewModel> GetByEmailAsync(string email);
        public Task<FamilyViewModel> GetByIdAsync(Guid id);
        public Task<List<FamilyViewModel>> GetAllAsync(string searchString);
        public Task<(int, List<FamilyViewModel>)> GetPagesAsync(FilterViewModel filter, string? sortBy, int pageIndex, int pageSize);
        public Task<CommandResult> CreateAsync(FamilyViewModel model);
        public Task<CommandResult> UpdateAsync(FamilyViewModel model);
        public Task<CommandResult> DeleteAsync(Guid id);
        public Task<CommandResult> DeleteAsyncNghia();
    }
}
