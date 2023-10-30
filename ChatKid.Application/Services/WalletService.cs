using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.WalletViewModels;
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
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletRepository;
        private readonly IMapper mapper;
        public WalletService(IWalletRepository walletRepository, IMapper mapper)
        {
            this.walletRepository = walletRepository;
            this.mapper = mapper;
        }
        public async Task<CommandResult> CreateAsync(WalletViewModel model)
        {
            Wallet result = null;
            model.UpdatedTime = DateTime.Now;
            model.Status = 1;

            try
            {
                result = await walletRepository.InsertAsync(mapper.Map<Wallet>(model), true);
                if (result is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = String.Format(CommandMessages.CreateFailed, "Wallet")
                });
            }
            catch(Exception ex)
            {
                Logger<WalletService>.Error(ex, ex.Message);
            }
            return CommandResult.SuccessWithData(result);
        }

        public async Task<CommandResult> DeleteAsync(Guid id)
        {
            try
            {
                var wallet = await walletRepository.GetByIdAsync(id);
                if (wallet is not null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = String.Format(CommandMessages.NotFound, id)
                });
                var result = await walletRepository.DeleteAsync(wallet);
                if(!result) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = String.Format(CommandMessages.DeleteFailed, "Wallet")
                });
            }
            catch (Exception ex)
            {
                Logger<WalletService>.Error(ex, ex.Message);
            }
            return CommandResult.SuccessWithData(id);
        }

        public async Task<List<WalletViewModel>> GetAllAsync(string searchString)
        {
            return mapper.Map<List<WalletViewModel>>(await walletRepository.TableNoTracking.Where(x => x.TotalEnergy.ToString().Contains(searchString)).ToListAsync());
        }

        public async Task<WalletViewModel> GetByIdAsync(Guid id)
        {
            return mapper.Map<WalletViewModel>(await walletRepository.TableNoTracking.Include("Transactions").SingleOrDefaultAsync(x => x.Id.Equals(id)));
        }

        public async Task<(int, List<WalletViewModel>)> GetPagesAsync(FilterViewModel filter, string? sortBy, int pageIndex, int pageSize)
        {
            var model = walletRepository.TableNoTracking.Where(x => x.TotalEnergy.ToString().Contains(filter.SearchString));
            if (!sortBy.IsNullOrEmpty())
            {
                model = model.Sort(sortBy);
            }
            var result = await model.Skip(pageIndex * pageSize).Take(pageSize).Include("Transactions").ToListAsync();
            return (model.Count(), mapper.Map<List<WalletViewModel>>(result));
        }

        public async Task<CommandResult> UpdateAsync(WalletViewModel model)
        {
            try
            {
                var subcription = await walletRepository.GetByIdAsync(model.Id);
                if (subcription is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, "Wallet")
                });

                var entity = mapper.Map<Wallet>(model);

                var result = await walletRepository.UpdateAsync(entity);
                if (!result)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Description = string.Format(CommandMessages.UpdateFailed, model.Id)
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
