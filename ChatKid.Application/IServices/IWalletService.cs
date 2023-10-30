using ChatKid.Application.Models.ViewModels.SubcriptionViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.WalletViewModels;

namespace ChatKid.Application.IServices
{
    public interface IWalletService
    {
        public Task<WalletViewModel> GetByIdAsync(Guid id);
        public Task<List<WalletViewModel>> GetAllAsync(string searchString);
        public Task<(int, List<WalletViewModel>)> GetPagesAsync(FilterViewModel filter, string? sortBy, int pageIndex, int pageSize);
        public Task<CommandResult> CreateAsync(WalletViewModel model);
        public Task<CommandResult> UpdateAsync(WalletViewModel model);
        public Task<CommandResult> DeleteAsync(Guid id);
    }
}
