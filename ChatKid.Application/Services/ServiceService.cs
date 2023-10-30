using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.ServiceRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Application.Models.ViewModels.ServiceViewModel;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> AddServiceAsync(ServiceCreateRequest model)
        {
            var service = _mapper.Map<Service>(model);
            if (service == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = "Service is null"
                });
            }
            var result = await _serviceRepository.InsertAsync(service);
            if (!result)
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(string.Format(CommandMessages.CreateFailed, service.Id))
                });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteServiceAsync(Guid id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            service.Status = 0;
            var result = await _serviceRepository.UpdateAsync(service);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            });
            return CommandResult.Success;
        }


        public async Task<ServiceViewModel> GetServiceAsync(Guid id)
        {
            return  _mapper.Map<ServiceViewModel>(await _serviceRepository.GetByIdAsync(id));
        }

        public async Task<(int, List<ServiceViewModel>)> GetServicePagesAsync(FilterViewModel filter, int pageIndex, int pageSize, string? sort)
        {
            IQueryable<Service> services;
            string search = filter.SearchString ?? "";
            if (search.IsNullOrEmpty())
            {
                services = _serviceRepository.TableNoTracking;
            }

            else
            {
                services = _serviceRepository.TableNoTracking
                                .Where(service => EF.Functions.ToTsVector("english", service.Name + " " + service.Type)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (service => EF.Functions.ToTsVector("english", service.Name + " " + service.Type)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }
            if (filter.Status != 2) services = services.Where(x => x.Status == filter.Status);
            if (!sort.IsNullOrEmpty())
            {
                services = services.Sort(sort);
            }
            var result = await services.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (services.Count(), _mapper.Map<List<ServiceViewModel>>(result));
        }

        public async Task<CommandResult> UpdateServiceAsync(Guid id, ServiceUpdateRequest model)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            _mapper.Map(model, service);
            var result = await _serviceRepository.UpdateAsync(service);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;
      
        }
    }
}
