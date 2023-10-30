using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.ExpertRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ExpertViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Identity;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class ExpertService : IExpertService
    {
        private readonly IExpertRepository _expertRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ExpertService(IExpertRepository expertRepository, IMapper mapper
            , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _expertRepository = expertRepository;
            this._mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CommandResult> AddExpertAsync(ExpertCreateRequest expertViewModel)
        {
            var expert = _mapper.Map<Expert>(expertViewModel);
            var duplicated = await _expertRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Gmail.Equals(expertViewModel.Gmail));
            if (duplicated != null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.Duplicated, expertViewModel.Gmail)
            });
            var phoneDuplicated = await _expertRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Phone.Equals(expertViewModel.Phone));
            if (phoneDuplicated != null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.Duplicated, expertViewModel.Phone)
            });
            ApplicationUser user = new ApplicationUser()
            {
                Email = expertViewModel.Gmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = expertViewModel.Gmail
            };
            await _userManager.CreateAsync(user);
            if (!await _roleManager.RoleExistsAsync(UserRoles.Expert))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Expert));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Expert);
            expert.ApplicationUserId = user.Id;

            var result = await _expertRepository.InsertAsync(expert);
            if (!result) return CommandResult.Failed(
                new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.CreateFailed, expert.Gmail)
                });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteExpertAsync(Guid id)
        {
            var expert = await _expertRepository.GetByIdAsync(id);
            if (expert == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            expert.Status = 0;
            var result = await _expertRepository.UpdateAsync(expert);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            });
            return CommandResult.Success;
        }

        public async Task<ExpertViewModel> GetExpertByIdAsync(Guid? id)
        {
            var expert = await _expertRepository.TableNoTracking.Where(x => x.Id.Equals(id)).Include(x => x.DiscussRooms.OrderByDescending(x => x.CreatedTime)).FirstOrDefaultAsync();
            return _mapper.Map<ExpertViewModel>(expert);
        }

        public async Task<ExpertViewModel> GetExpertByEmailAsync(string mail)
        {
            var expert = await _expertRepository.GetExpertByGmailAsync(mail);
            return _mapper.Map<ExpertViewModel>(expert);
        }


        public async Task<(int, List<ExpertViewModel>)> GetExpertPagesAsync(FilterViewModel filter, int pageIndex, int pageSize, string? sort)
        {

            if (!filter.Email.IsNullOrEmpty())
            {
                var email = filter.Email;

                var expert = _expertRepository.TableNoTracking
                    .Where(x => email.Equals(x.Gmail));

                var resultEmail = await expert
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (expert.Count(), _mapper.Map<List<ExpertViewModel>>(resultEmail));
            }

            IQueryable<Expert> experts;
            string search = filter.SearchString ?? "";
            if (search.IsNullOrEmpty())
            {
                experts = _expertRepository.TableNoTracking;
            }

            else
            {
                experts = _expertRepository.TableNoTracking
                                .Where(expert => EF.Functions.ToTsVector("english", expert.FirstName + " " + expert.LastName + " " + expert.Gmail)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (expert => EF.Functions.ToTsVector("english", expert.FirstName + " " + expert.LastName + " " + expert.Gmail)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }


            if (filter.Status != 2) experts = experts.Where(x => x.Status == filter.Status);


            if (!sort.IsNullOrEmpty())
            {
                experts = experts.Sort(sort);
            }
            else
            {
                experts = experts.OrderByDescending(x => x.UpdatedAt);
            }
            var result = await experts.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (experts.Count(), _mapper.Map<List<ExpertViewModel>>(result));
        }

        public async Task<CommandResult> UpdateExpertAsync(Guid id, ExpertUpdateRequest expertViewModel)
        {
            var expert = await _expertRepository.GetByIdAsync(id);
            if (expert == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            _mapper.Map(expertViewModel, expert);
            var result = await _expertRepository.UpdateAsync(expert);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;
        }

        public async Task<ExpertViewModel> GetExpertInfo(string? mail, string? avatar)
        {
            var expert = await _expertRepository.GetExpertByGmailAsync(mail);
            if (expert.AvatarUrl.IsNullOrEmpty())
            {
                expert.AvatarUrl = avatar;
                bool ok = await _expertRepository.UpdateAsync(expert);
            }
            var expertViewModel = _mapper.Map<ExpertViewModel>(expert);
            return expertViewModel;
        }
    }
}
