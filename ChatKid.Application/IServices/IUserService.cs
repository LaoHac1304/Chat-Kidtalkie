using ChatKid.Application.Models.ViewModels.UserViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IUserService
    {
        Task<UserViewModel> UserLogin(Guid userId, string password);
        Task<UserFamilyViewModel> GetIdAsync(Guid id);
        Task<List<UserFamilyViewModel>> GetAllAsync(Guid familyId);
        Task<CommandResult> CreateAsync(UserViewModel model);
        Task<CommandResult> CreateEmptyUsersAsync(Guid familyId);
        Task<CommandResult> UpdateAsync(UserViewModel model);
        Task<CommandResult> DeleteAsync(Guid id);
        Task<CommandResult> DeleteFamilyUsersAsync(Guid familyId);
    }
}
