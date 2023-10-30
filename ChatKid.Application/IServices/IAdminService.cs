using ChatKid.Application.Models.RequestModels.Admin;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdminViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IAdminService
    {
        public Task<CommandResult> Create(AdminCreateRequest model);
        public Task<CommandResult> DeleteAsync(Guid id);
        public Task<CommandResult> DeleteByEmail(string email);
        public Task<AdminViewModel> GetAdminInfo(string? email, string? avatar);
        public Task<List<AdminViewModel>> GetAllAdmin();
        public Task<AdminViewModel> GetByEmail(string email);
        public Task<AdminViewModel> GetByIdAsync(Guid id);
        public Task<(int, List<AdminViewModel>)> GetPagesAsync(FilterViewModel filter
            , int pageNumber, int pageSize, string sortBy);
        public Task<CommandResult> Update(Guid id, AdminUpdateRequest adminViewModel);
    }
}
