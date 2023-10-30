using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.KidServiceRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.KidServiceViewModel;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class KidServiceService : IKidServiceService
    {
        private readonly IKidServiceRepository _kidServiceRepository;
        private readonly IMapper _mapper;

        public KidServiceService(IKidServiceRepository kidServiceRepository, IMapper mapper)
        {
            _kidServiceRepository = kidServiceRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> CreateKidServiceAsync(KidServiceCreateRequest kidServiceViewModel)
        {
            var kidService = _mapper.Map<KidService>(kidServiceViewModel);
            if (kidService == null) return CommandResult.Failed(new CommandResultError()
                { Code = (int)HttpStatusCode.BadRequest,
                Description = "KidService is null" });
            var result = await _kidServiceRepository.InsertAsync(kidService);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(string.Format(CommandMessages.CreateFailed, kidService.Id))
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteKidServiceAsync(Guid id)
        {
            var kidService = await _kidServiceRepository.GetByIdAsync(id);
            if (kidService ==null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "KidService is null"
            });
            kidService.Status = 0;
            var result = await _kidServiceRepository.UpdateAsync(kidService);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(string.Format(CommandMessages.DeleteFailed, kidService.Id))
            });
            return CommandResult.Success;
        }


        public async Task<KidServiceViewModel> GetKidServiceAsync(Guid id)
        {
            var kidService = await _kidServiceRepository.GetByIdAsync(id);
            return _mapper.Map<KidServiceViewModel>(kidService);
        }

        public async Task<(int, IEnumerable<KidServiceViewModel>)> GetKidServicePagesAsync(FilterViewModel filter, int pageIndex, int pageSize)
        {
            var kidServices = _kidServiceRepository.TableNoTracking;
            var result = await kidServices.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (kidServices.Count(), _mapper.Map<IEnumerable<KidServiceViewModel>>(result));
        }

        public async Task<CommandResult> UpdateKidServiceAsync(Guid id, KidServiceUpdateRequest kidServiceViewModel)
        {
            var kidService = await _kidServiceRepository.GetByIdAsync(id);
            if (kidService == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            _mapper.Map(kidServiceViewModel, kidService);
            var result = await _kidServiceRepository.UpdateAsync(kidService);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;
        }
    }
}
