using ChatKid.Application.Models.RequestModels.ServiceRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ServiceViewModel;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IServiceService
    {
        Task<CommandResult> AddServiceAsync(ServiceCreateRequest model);
        Task<CommandResult> DeleteServiceAsync(Guid id);
        Task<CommandResult> UpdateServiceAsync(Guid id, ServiceUpdateRequest model);
        Task<ServiceViewModel> GetServiceAsync(Guid id);
        Task<(int, List<ServiceViewModel>)> GetServicePagesAsync(FilterViewModel filter, int pageIndex, int pageSize, string? sort);
    }
}
