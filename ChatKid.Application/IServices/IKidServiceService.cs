using ChatKid.Application.Models.RequestModels.KidServiceRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.KidServiceViewModel;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IKidServiceService
    {
        Task <KidServiceViewModel> GetKidServiceAsync(Guid id);
        Task <(int, IEnumerable<KidServiceViewModel>)> GetKidServicePagesAsync(FilterViewModel filter, int pageIndex, int pageSize);
        Task <CommandResult> CreateKidServiceAsync(KidServiceCreateRequest kidServiceViewModel);
        Task <CommandResult> UpdateKidServiceAsync(Guid id, KidServiceUpdateRequest kidServiceViewModel);
        Task <CommandResult> DeleteKidServiceAsync(Guid id);
    }
}
