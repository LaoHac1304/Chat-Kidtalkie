using ChatKid.Application.Models.FilterModels;
using ChatKid.Application.Models.RequestModels.DiscussRoom;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.DiscussRoomViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.IServices
{
    public interface IDiscussRoomService
    {
        Task<CommandResult> CreateDiscussRoomAsync(DiscussRoomCreateRequest request);
        Task<CommandResult> UpdateDiscussRoomAsync(Guid id, DiscussRoomUpdateRequest request);
        Task<CommandResult> DeleteDiscussRoomAsync(Guid id);
        Task<(int, List<DiscussRoomViewModel>)> GetDiscussRoomPagesAsync(FilterViewModel filterViewModel, int pageIndex, int pageSize, string? sort, DiscussRoomFilter roomFilter);
        Task<DiscussRoomViewModel> GetDiscussRoomByIdAsync(Guid id);
    }
}
