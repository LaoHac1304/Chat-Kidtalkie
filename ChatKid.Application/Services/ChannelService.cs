using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IMapper _mapper;

        public ChannelService(IChannelRepository channelRepository
            , IChannelUserRepository channelUserRepository
            ,IMapper mapper)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
            _channelUserRepository = channelUserRepository;
        }

        public async Task<CommandResult> AddUsers(AddUsersViewModel model)
        {
            var channel =  await _channelRepository.TableNoTracking
                .SingleOrDefaultAsync(c => c.Id == model.ChannelId);
            if (channel is null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, model.ChannelId)
                }) ;
            }

            var channelUsers = model.UserIds.Select(userId => new ChannelUser
            {
                ChannelId = channel.Id,
                UserId = userId,
            });

            bool result = await _channelUserRepository.InsertAsync(channelUsers);
            if (!result)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.CreateFailed)
                });
            }
            return CommandResult.Success;
        }

        public async Task<CommandResult> CreateChannel(ChannelCreateViewModel model)
        {

            if (model.Name.IsNullOrEmpty() 
                || model.UserIds.IsNullOrEmpty())
                return CommandResult.Failed(new CommandResultError(){
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = string.Format(CommandMessages.CreateFailed, model.Name)
                });

            model.Status = 1;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            var channel = _mapper.Map<Channel>(model);


            var result = await _channelRepository.InsertAsync(channel,true);

            if (result is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.CreateFailed, model.Name)
            });

            // insert channelUsers
            var channelUsers = model.UserIds.Select(cU => new ChannelUser
            {
                ChannelId = result.Id,
                UserId = cU,
                NameInChannel = result.Name,
                Status = 1
            }).ToList();

            await _channelUserRepository.InsertAsync(channelUsers);

            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteChannel(Guid id)
        {
            var channel = await _channelRepository.TableNoTracking
                .SingleOrDefaultAsync(c => c.Status == 1 && c.Id == id);

            if (channel is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = "Channel not found in system",
            });

            channel.Status = 0;
            await _channelRepository.DeleteAsync(channel);
            return CommandResult.Success;
        }

        public async Task<(int, List<ChannelViewModel>)> GetAllChannel(FilterViewModel filter
            , int pageIndex, int pageSize)
        {
            if (filter.SearchString is null) 
                filter.SearchString = "";

            var listChannel = _channelRepository.TableNoTracking
                .Where(c => c.Name.Contains(filter.SearchString) 
                && c.Status.Equals(filter.Status));

            var result = await listChannel
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include("ChannelUsers")
                .ToListAsync();

            return (listChannel.Count(), _mapper.Map<List<ChannelViewModel>>(result));
        }


        public async Task<ChannelViewModel> GetChannelById(Guid id)
        {
            var channel = await _channelRepository.TableNoTracking
                .Include(c => c.ChannelUsers)
                .SingleOrDefaultAsync (c => c.Id == id);

            var result = _mapper.Map<ChannelViewModel>(channel);

            return result;
        }

        public async Task<CommandResult> UpdateChannel(ChannelViewModel model)
        {

            var channel = await _channelRepository.TableNoTracking
                .SingleOrDefaultAsync(c => c.Id == model.Id);


            if (channel is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, model.Id)
            });
            if (!model.Name.IsNullOrEmpty())
                channel.Name = model.Name;
            if (model.Status != 1)
            {
                channel.Status = model.Status ;
            }
            await _channelRepository.UpdateAsync(channel);
            return CommandResult.SuccessWithData(model);
        }
    }
}
