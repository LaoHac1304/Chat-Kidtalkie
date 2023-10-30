using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.FilterModels;
using ChatKid.Application.Models.RequestModels.DiscussRoom;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.DiscussRoomViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class DiscussRoomService : IDiscussRoomService
    {
        private readonly IDiscussRoomRepository _discussRoomRepository;
        private readonly IMapper _mapper;

        public DiscussRoomService(IDiscussRoomRepository discussRoomRepository, IMapper mapper)
        {
            _discussRoomRepository = discussRoomRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> CreateDiscussRoomAsync(DiscussRoomCreateRequest request)
        {
            var discussRoom = _mapper.Map<DiscussRoom>(request);
            if (discussRoom == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = "Discuss room is null"
                });
            }
            var result = await _discussRoomRepository.InsertAsync(discussRoom);
            if (!result)
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(string.Format(CommandMessages.CreateFailed, discussRoom.Id))
                });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteDiscussRoomAsync(Guid id)
        {
            var discussRoom = await _discussRoomRepository.GetByIdAsync(id);
            if (discussRoom == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            discussRoom.Status = 0;
            var result = await _discussRoomRepository.UpdateAsync(discussRoom);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            });
            return CommandResult.Success;
        }

        public async Task<DiscussRoomViewModel> GetDiscussRoomByIdAsync(Guid id)
        {
            var discussRoom = await _discussRoomRepository.TableNoTracking.Where(x => x.Id.Equals(id)).Include(x => x.KidService.Children).FirstOrDefaultAsync();
            return _mapper.Map<DiscussRoomViewModel>(discussRoom);
        }

        public async Task<(int, List<DiscussRoomViewModel>)> GetDiscussRoomPagesAsync(FilterViewModel filterViewModel, int pageIndex, int pageSize, string? sort, DiscussRoomFilter roomFilter)
        {
            IQueryable<DiscussRoom> discussRooms;
            string search = filterViewModel.SearchString ?? "";
            if (search.IsNullOrEmpty())
            {
                discussRooms = _discussRoomRepository.TableNoTracking;
            }

            else
            {
                discussRooms = _discussRoomRepository.TableNoTracking
                                .Where(discussRoom => EF.Functions.ToTsVector("english", discussRoom.Expert.FirstName + " " + discussRoom.Expert.LastName + " " + discussRoom.KidService.Children.Name)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (discussRoom => EF.Functions.ToTsVector("english", discussRoom.Expert.FirstName + " " + discussRoom.Expert.LastName + " " + discussRoom.KidService.Children.Name)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }
            typeof(DiscussRoomFilter).GetProperties().ToList().ForEach(property =>
            {
                var value = property.GetValue(roomFilter);
                if (value != null)
                {
                    discussRooms = discussRooms.Where(x => x.GetType().GetProperty(property.Name).GetValue(x).Equals(value));
                }
            });
            if (filterViewModel.Status != 2) discussRooms = discussRooms.Where(x => x.Status == filterViewModel.Status);
            if (!sort.IsNullOrEmpty())
            {
                discussRooms = discussRooms.Sort(sort);
            }
            discussRooms = discussRooms.Include(x => x.KidService.Children);
            var result = await discussRooms.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (discussRooms.Count(), _mapper.Map<List<DiscussRoomViewModel>>(result));
        }

        public async Task<CommandResult> UpdateDiscussRoomAsync(Guid id, DiscussRoomUpdateRequest request)
        {
            var discussRoom = await _discussRoomRepository.GetByIdAsync(id);
            if (discussRoom == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            _mapper.Map(request, discussRoom);
            var result = await _discussRoomRepository.UpdateAsync(discussRoom);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;
        }
    }
}
