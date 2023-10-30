using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.MessageViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository
            , IMapper mapper
            , IChannelUserRepository channelUserRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _channelUserRepository = channelUserRepository;
        }

        public async Task<CommandResult> AddMessage(MessageViewModel model)
        {
            /*model.Status = 1;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;
            model.Status = 1;*/

            var message = _mapper.Map<Message>(model);
            message.SentTime = DateTime.UtcNow;
            message.Status = model.Status;

            var checkExistChannelUserId = _channelUserRepository.TableNoTracking
                .SingleOrDefault(c => c.Id.Equals(model.ChannelUserId));
            if (checkExistChannelUserId == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "channel user id is not founded"
            });


            var result = await _messageRepository.InsertAsync(message);

            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.CreateFailed)
            });

            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteMessage(Guid id)
        {
            var message = await _messageRepository.TableNoTracking
                        .SingleOrDefaultAsync(m => m.Id == id);
            if (message is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.DeleteFailed)
            });

            message.Status = 0;

            var result = await _messageRepository.UpdateAsync(message);

            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.DeleteFailed)
            });

            return CommandResult.Success;
        }

        public async Task<(int, List<MessageViewModel>)> GetAllMessage(FilterViewModel filter
            , int pageIndex, int pageSize)
        {

            if (filter.SearchString is null)
                filter.SearchString = "";

            var listMessage = _messageRepository.TableNoTracking
                .Where(m => m.Content.Contains(filter.SearchString)
                            && m.Status == filter.Status);

            var result = await listMessage
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (listMessage.Count(), _mapper.Map<List<MessageViewModel>>(result));
        }

        public async Task<MessageViewModel> GetMessageById(Guid id)
        {
            var channel = await _messageRepository.TableNoTracking
            .SingleOrDefaultAsync(m => m.Id == id);

            var result = _mapper.Map<MessageViewModel>(channel);

            return result;
        }

        public async Task<CommandResult> UpdateMessage(MessageViewModel model)
        {
            var message = await _messageRepository.TableNoTracking
                .SingleOrDefaultAsync(m => m.Id == model.Id);


            if (message is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, model.Id)
            });

           
            if (model.Status != 1)
            {
                message.Status = model.Status;
            }
            await _messageRepository.UpdateAsync(message);
            return CommandResult.SuccessWithData(model);
        }
    }
}
