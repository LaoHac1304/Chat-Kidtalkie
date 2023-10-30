using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelUserViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IChannelUserService
    {
        public Task<CommandResult> CreateChannelUser(ChannelUserViewModel model);
        public Task<CommandResult> DeleteChannelUser(Guid id);
        public Task<(int, List<ChannelUserViewModel>)> GetAllChannelUser
            (FilterViewModel filter, int pageIndex, int pageSize);
        public Task<ChannelUserViewModel> GetChannelUserById(Guid id);
        public Task<CommandResult> UpdateById(ChannelUserViewModel model);
    }
}
