using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelUserViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class ChannelUserService : IChannelUserService
    {
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ChannelUserService(IChannelUserRepository channelUserRepository
            , IChannelRepository channelRepository
            , IUserRepository userRepository
            , IMapper mapper)
        {
            _channelUserRepository = channelUserRepository;
            _mapper = mapper;
            _channelRepository = channelRepository;
            _userRepository = userRepository;
        }

        public async Task<CommandResult> CreateChannelUser(ChannelUserViewModel model)
        {

            model.Status = 1;
            /*model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;*/

            var validChannelId = _channelRepository
                .TableNoTracking
                .SingleOrDefaultAsync(c => c.Id == model.ChannelId);

            if (validChannelId == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "channel Id is not valid"
            });

            var validUserId = _userRepository
                .TableNoTracking
                .SingleOrDefaultAsync(u => u.Id == model.UserId);

            if (validChannelId == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "channel Id is not valid"
            });

            var isExistUser = _channelUserRepository
                .TableNoTracking
                .SingleOrDefaultAsync(c => c.ChannelId == model.ChannelId && c.UserId == model.UserId);

            if (isExistUser is not null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "user is exist in channel"
            });

            var channel = _mapper.Map<ChannelUser>(model);

            var result = await _channelUserRepository.InsertAsync(channel);

            if (result is false) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.CreateFailed)
            });

            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteChannelUser(Guid id)
        {
            var channelUser = await _channelUserRepository.TableNoTracking
                .SingleOrDefaultAsync(c => c.Status == 1 && c.Id == id);

            if (channelUser is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = "User not found in channel",
            });

            channelUser.Status = 0;
            await _channelUserRepository.UpdateAsync(channelUser);
            return CommandResult.Success;
        }

        public async Task<(int, List<ChannelUserViewModel>)> 
            GetAllChannelUser(FilterViewModel filter, int pageIndex, int pageSize)
        {
            if (filter.SearchString is null)
                filter.SearchString = "";

            var listChannelUser = _channelUserRepository.TableNoTracking
                .Where(c => c.NameInChannel.Contains(filter.SearchString)
                && c.Status.Equals(filter.Status));

            var result = await listChannelUser
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (listChannelUser.Count(), _mapper.Map<List<ChannelUserViewModel>>(result));
        }

        public async Task<ChannelUserViewModel> GetChannelUserById(Guid id)
        {
            var channel = await _channelUserRepository.TableNoTracking
                .SingleOrDefaultAsync(c => c.Id == id);

            var result = _mapper.Map<ChannelUserViewModel>(channel);

            return result;
        }

        public async Task<CommandResult> UpdateById(ChannelUserViewModel model)
        {
            var channerUsers = await _channelUserRepository.TableNoTracking
                .SingleOrDefaultAsync(cu => cu.Id == model.Id);

            if (channerUsers is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.UpdateFailed)
            });

            channerUsers.NameInChannel = model.NameInChannel;

            var result = await _channelUserRepository.UpdateAsync(channerUsers);

            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed)
            });

            return CommandResult.Success;
        }
    }
}
