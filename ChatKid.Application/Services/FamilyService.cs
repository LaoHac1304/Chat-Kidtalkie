using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.FamilyViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.Common.Logger;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Identity;
using ChatKid.DataLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IUserService userService;
        private readonly IFamilyRepository familyRepository;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public FamilyService(
            IUserService userService, 
            IFamilyRepository familyRepository, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            this.familyRepository = familyRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CommandResult> CreateAsync(FamilyViewModel model)
        {
            Family result = null;
            CommandResult response = null;
            try
            {
                var duplicated = await familyRepository.TableNoTracking.SingleOrDefaultAsync(x => x.OwnerMail == model.OwnerMail);
                if (duplicated is not null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = string.Format("Email Has Created Family")
                });

                model.Status = 1;
                model.CreatedAt = DateTime.UtcNow;
                model.UpdatedAt = DateTime.UtcNow;

                //create role
                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.OwnerMail,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.OwnerMail
                };

                if (!await _roleManager.RoleExistsAsync(UserRoles.Parent))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Parent));
                }

                var resultApp = await _userManager.CreateAsync(user);
                if (!resultApp.Succeeded) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.CreateFailed, model.OwnerMail)
                });

                var roleResponse = await _userManager.AddToRoleAsync(user, UserRoles.Parent);
                if (!resultApp.Succeeded) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.CreateFailed, model.OwnerMail)
                });

                var entity = _mapper.Map<Family>(model);
                entity.ApplicationUserId = user.Id;
                //

                result = await familyRepository.InsertAsync(entity, true);
                if (result is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.CreateFailed, model.OwnerMail)
                });

                response = await userService.CreateEmptyUsersAsync(result.Id);
                if (!response.Succeeded)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = response.GetStatusCode(),
                        Description = response.GetFirstErrorMessage()
                    });
                }
            }
            catch (Exception ex)
            {
                Logger<FamilyService>.Error(ex, ex.Message);
            }

            return CommandResult.SuccessWithData(_mapper.Map<FamilyViewModel>(result));
        }

        public async Task<CommandResult> DeleteAsync(Guid id)
        {
            try
            {
                var model = await familyRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id.Equals(id));
                if (model == null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });

                var deleteUsers = await userService.DeleteFamilyUsersAsync(model.Id);
                if (!deleteUsers.Succeeded) return CommandResult.Failed(new CommandResultError()
                {
                    Code = deleteUsers.GetStatusCode(),
                    Description = deleteUsers.GetFirstErrorMessage()
                });

                

                bool result = await familyRepository.DeleteAsync(model);
                
                if (!result) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = string.Format(CommandMessages.DeleteFailed, id)
                });
                var applicationId = model.ApplicationUserId;
                var userApp = await _userManager.FindByIdAsync(applicationId);
                await _userManager.DeleteAsync(userApp);
            }
            catch(Exception ex)
            {
                Logger<FamilyService>.Error(ex, ex.Message);
            }
            
            return CommandResult.SuccessWithData(id);
        }

        public async Task<CommandResult> DeleteAsyncNghia()
        {
            try
            {
                var model = await familyRepository.TableNoTracking
                    .Where(x => x.OwnerMail.Contains("nghia")).ToListAsync();

                if (model == null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound)
                });

                foreach (var family in model)
                {
                    var familyId = family.Id;
                    var deleteUsers = await userService.DeleteFamilyUsersAsync(familyId);
                    if (!deleteUsers.Succeeded) return CommandResult.Failed(new CommandResultError()
                    {
                        Code = deleteUsers.GetStatusCode(),
                        Description = deleteUsers.GetFirstErrorMessage()
                    });


                    bool result = await familyRepository.DeleteAsync(family);
                    
                    if (!result) return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Description = string.Format(CommandMessages.DeleteFailed)
                    });
                    var applicationId = family.ApplicationUserId;
                    var userApp = await _userManager.FindByIdAsync(applicationId);
                    await _userManager.DeleteAsync(userApp);

                }
            }
            catch (Exception ex)
            {
                Logger<FamilyService>.Error(ex, ex.Message);
            }

            return CommandResult.Success;
        }

        public async Task<List<FamilyViewModel>> GetAllAsync(string searchString)
        {
            return _mapper.Map<List<FamilyViewModel>>(await familyRepository.TableNoTracking.Where(x => x.Name.Contains(searchString) || x.OwnerMail.Contains(searchString)).ToListAsync());
        }

        public async Task<FamilyViewModel> GetByEmailAsync(string email)
        {
            var result = await familyRepository.TableNoTracking.Include("Users").Include("ParentSubcriptions")
                                                .SingleOrDefaultAsync(x => x.OwnerMail.Equals(email));
            return _mapper.Map<FamilyViewModel>(result);
        }

        public async Task<FamilyViewModel> GetByIdAsync(Guid id)
        {
            var result = await familyRepository.TableNoTracking.Include("Users").Include("ParentSubcriptions")
                                                .SingleOrDefaultAsync(x => x.Id.Equals(id));
            return _mapper.Map<FamilyViewModel>(result);
        }

        public async Task<(int, List<FamilyViewModel>)> GetPagesAsync(FilterViewModel filter, string? sortBy, int pageIndex, int pageSize)
        {

            if (!filter.Email.IsNullOrEmpty())
            {
                var email = filter.Email;

                var family = familyRepository.TableNoTracking
                    .Where(x => email.Equals(x.OwnerMail));

                var resultEmail = await family
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (family.Count(), _mapper.Map<List<FamilyViewModel>>(resultEmail));
            }

            var model = familyRepository.TableNoTracking.Include("Users").Include("ParentSubcriptions")
                .Where(x => (x.OwnerMail.Contains(filter.SearchString) || x.Name.Contains(filter.SearchString)));
            if (filter.Status != 2) model = model.Where(x => x.Status == filter.Status);
            if (!sortBy.IsNullOrEmpty())
            {
                model = model.Sort(sortBy);
            }
            var result = await model.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (model.Count(), _mapper.Map<List<FamilyViewModel>>(result));
        }

        public async Task<CommandResult> UpdateAsync(FamilyViewModel model)
        {
            try
            {
                var family = await familyRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
                if (family is null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, "Family")
                });

                _mapper.Map(family, model);

                var result = await familyRepository.UpdateAsync(family);
                if (!result)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Description = string.Format(CommandMessages.UpdateFailed, model.OwnerMail)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger<FamilyService>.Error(ex, ex.Message);
            }
            
            return CommandResult.SuccessWithData(model);
        }
    }
}
