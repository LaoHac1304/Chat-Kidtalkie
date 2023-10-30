using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.MessageViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IMessageService
    {
        public Task<CommandResult> AddMessage(MessageViewModel model);
        public Task<CommandResult> DeleteMessage(Guid id);
        public Task<(int, List<MessageViewModel>)> GetAllMessage(FilterViewModel filter
            , int pageIndex, int pageSize);
        public Task<MessageViewModel> GetMessageById(Guid id);
        public Task<CommandResult> UpdateMessage(MessageViewModel model);
    }
}
