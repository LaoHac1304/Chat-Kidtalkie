using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IChannelService
    {
        public Task<CommandResult> AddUsers(AddUsersViewModel model);
        public Task<CommandResult> 
            CreateChannel(ChannelCreateViewModel model);
        public Task<CommandResult> DeleteChannel(Guid id);
        public Task<(int, List<ChannelViewModel>)> GetAllChannel(FilterViewModel filter
            , int pageIndex, int pageSize);
        public Task<ChannelViewModel> GetChannelById(Guid id);
        public Task<CommandResult> UpdateChannel(ChannelViewModel model);
    }
}
