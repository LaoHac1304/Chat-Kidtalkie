using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.SubcriptionViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.Common.Logger;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class SubcriptionService : ISubcriptionService
    {
        private readonly ISubcriptionRepository subcriptionRepository;
        private readonly IMapper _mapper;

        public SubcriptionService(ISubcriptionRepository subcriptionRepository, IMapper mapper)
        {
            this.subcriptionRepository = subcriptionRepository;
            this._mapper = mapper;
        }

        public async Task<CommandResult> CreateAsync(SubcriptionViewModel model)
        {
            Subcription result = null;
            model.Status = 1;
            try
            {
                result = await subcriptionRepository.InsertAsync(_mapper.Map<Subcription>(model), true);
                if (result is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = String.Format(CommandMessages.CreateFailed, model.Name)
                });
            }
            catch(Exception ex)
            {
                Logger<SubcriptionService>.Error(ex, ex.Message);
            }
            return CommandResult.SuccessWithData(result);
        }

        public async Task<CommandResult> DeleteAsync(Guid id)
        {
            try
            {
                var model = await subcriptionRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id.Equals(id));
                if (model is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = String.Format(CommandMessages.NotFound, id)
                });
                var result = await subcriptionRepository.DeleteAsync(model);
                if(!result) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = String.Format(CommandMessages.DeleteFailed, id)
                });
            }
            catch(Exception ex)
            {
                Logger<SubcriptionService>.Error(ex, ex.Message);
            }
            return CommandResult.SuccessWithData(id);
        }

        public async Task<List<SubcriptionViewModel>> GetAllAsync(string searchString)
        {
            return _mapper.Map<List<SubcriptionViewModel>>(await subcriptionRepository.TableNoTracking.Where(x => x.Name.Contains(searchString)).ToListAsync());
        }

        public async Task<SubcriptionViewModel> GetByIdAsync(Guid id)
        {
            return _mapper.Map<SubcriptionViewModel>(await subcriptionRepository.TableNoTracking.SingleOrDefaultAsync());
        }

        public async Task<(int, List<SubcriptionViewModel>)> GetPagesAsync(FilterViewModel filter, string? sortBy, int pageIndex, int pageSize)
        {
            var model = subcriptionRepository.TableNoTracking.Where(x => x.Name.Contains(filter.SearchString));
            if (!sortBy.IsNullOrEmpty())
            {
                model = model.Sort(sortBy);
            }
            var result = await model.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (model.Count(), _mapper.Map<List<SubcriptionViewModel>>(result));
        }

        public async Task<CommandResult> UpdateAsync(SubcriptionViewModel model)
        {
            try
            {
                var subcription = await subcriptionRepository.GetByIdAsync(model.Id);
                if (subcription is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, "Subcription")
                });

                var entity = _mapper.Map<Subcription>(model);

                var result = await subcriptionRepository.UpdateAsync(entity);
                if (!result)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Description = string.Format(CommandMessages.UpdateFailed, model.Name)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger<SubcriptionService>.Error(ex, ex.Message);
            }
            return CommandResult.SuccessWithData(model);
        }
    }
}
