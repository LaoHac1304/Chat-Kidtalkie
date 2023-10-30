using ChatKid.Application.Models.RequestModels.NotificationRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.NotificationViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface INotificationService
    {
        public Task<CommandResult> AddNotificationAsync(NotificationCreateRequest model); 
        public Task<CommandResult> DeleteNotificationAsync(Guid id); 
        public Task<CommandResult> UpdateNotificationAsync(Guid id, NotificationUpdateRequest model);
        public Task<NotificationViewModel> GetNotificationAsync(Guid id);
        public Task<(int, List<NotificationViewModel>)> GetNotificationPagesAsync(FilterViewModel filter, int pageIndex, int pageSize, string? sort);
    }
}
