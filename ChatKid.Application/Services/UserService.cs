using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels.UserViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Identity;
using ChatKid.DataLayer.Repositories.Interfaces;
using ChatKid.DataLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IFamilyRepository familyRepository;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository,
            IFamilyRepository familyRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.familyRepository = familyRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> CreateAsync(UserViewModel model)
        {
            model.Status = 1;
            model.IsUpdated = 0;
            var entity = _mapper.Map<User>(model);


            //check duplicated
            var duplicated = await userRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Name == model.Name);
            if (duplicated is not null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Conflict,
                Description = string.Format(CommandMessages.Duplicated, model.Name)
            });

            //check existed family
            var family = await familyRepository.GetByIdAsync(entity.FamilyId);
            if (family == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, "Family")
            });

            //check family valid
            bool isValid = await userRepository.IsFullFamilyUser(entity.FamilyId ?? Guid.Empty);
            if (!isValid) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format("Family is full")
            });

            //insert async
            var result = await userRepository.InsertAsync(entity);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.CreateFailed, model.Name)
            });
            return CommandResult.SuccessWithData(model);
        }

        public async Task<CommandResult> CreateEmptyUsersAsync(Guid familyId)
        {
            List<UserViewModel> users = new()
            {
                new UserViewModel { 
                    AvatarUrl = string.Empty,
                    Name = "Tài khoản phụ huynh 1",
                    Password = string.Empty,
                    Role = UserRoles.Parent,
                    DeviceToken = string.Empty,
                    FamilyId = familyId,
                    Status = 1,
                    IsUpdated = 0
                },
                new UserViewModel {
                    AvatarUrl = string.Empty,
                    Name = "Tài khoản phụ huynh 2",
                    Password = string.Empty,
                    Role = UserRoles.Parent,
                    DeviceToken = string.Empty,
                    FamilyId = familyId,
                    Status = 1,
                    IsUpdated = 0
                },
                new UserViewModel {
                    AvatarUrl = string.Empty,
                    Name = "Tài khoản bé 1",
                    Password = string.Empty,
                    Role = UserRoles.Children,
                    DeviceToken = string.Empty,
                    FamilyId = familyId,
                    Status = 1,
                    IsUpdated = 0
                },
                new UserViewModel {
                    AvatarUrl = string.Empty,
                    Name = "Tài khoản bé 2",
                    Password = string.Empty,
                    Role = UserRoles.Children,
                    DeviceToken = string.Empty,
                    FamilyId = familyId,
                    Status = 1,
                    IsUpdated = 0
                },
                new UserViewModel {
                    AvatarUrl = string.Empty,
                    Name = "Tài khoản bé 3",
                    Password = string.Empty,
                    Role = UserRoles.Children,
                    DeviceToken = string.Empty,
                    FamilyId = familyId,
                    Status = 1,
                    IsUpdated = 0
                }
            };

            var entities = _mapper.Map<List<User>>(users);

            var response = await userRepository.InsertAsync(entities.AsEnumerable());
            if(!response) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.CreateFailed, "User Profile")
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteAsync(Guid id)
        {
            var model = await userRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (model == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });

            bool result = await userRepository.DeleteAsync(model);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            });
            return CommandResult.SuccessWithData(id);
        }

        public async Task<CommandResult> DeleteFamilyUsersAsync(Guid familyId)
        {
            var users = await userRepository.TableNoTracking
                .Where(x => x.FamilyId.Equals(familyId)).ToListAsync();
            if (users is null || users.Count == 0) return CommandResult.Success;
            var result = await userRepository.DeleteAsync(users);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.DeleteFailed, "Users")
            });
            return CommandResult.Success;
        }

        public async Task<List<UserFamilyViewModel>> GetAllAsync(Guid familyId)
        {
            var result = await userRepository.TableNoTracking.Where(x => x.FamilyId.Equals(familyId))
                .Include("Family")
                .ToListAsync();
            return _mapper.Map<List<UserFamilyViewModel>>(result);
        }

        public async Task<UserFamilyViewModel> GetIdAsync(Guid id)
        {
            var response = await userRepository.GetByIdAsync(id);
            return _mapper.Map<UserFamilyViewModel>(response);
        }

        public async Task<CommandResult> UpdateAsync(UserViewModel model)
        {
            model.IsUpdated = 1;
            var user = await userRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id.Equals(model.Id));
            if(user is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, model.Name)
            });

            _mapper.Map(model, user);

            var result = await userRepository.UpdateAsync(user);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, model.Name)
            });
            return CommandResult.SuccessWithData(model);
        }

        public async Task<UserViewModel> UserLogin(Guid userId, string? password)
        {
            return _mapper.Map<UserViewModel>(await userRepository.LoginWithPassword(userId, password));
        }
    }
}