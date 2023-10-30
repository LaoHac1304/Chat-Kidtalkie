using ChatKid.Application.Models.FilterModels;
using ChatKid.Application.Models.RequestModels.Advertising;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdvertisingViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IAdvertisingService
    {
        public Task<CommandResult> AddAdvertisingAsync(AdvertisingCreateRequest model);
        public Task<CommandResult> DeleteAdvertisingAsync(Guid id);
        public Task<CommandResult> UpdateAdvertisingAsync(Guid id, AdvertisingUpdateRequest model);
        public Task<CommandResult> ClickAdvertisingAsync(Guid id);
        public Task<AdvertisingDetailViewModel> GetAdvertisingAsync(Guid id);
        public Task<List<AdvertisingViewModel>> GetAllAdvertisingAsync(String search);
        public Task<(int, List<AdvertisingDetailViewModel>)> GetAdvertisingPagesAsync
            (FilterViewModel filterViewModel, AdvertisingFilter advertisingFilter, int pageIndex, int pageSize, string sortBy);
    }
}
