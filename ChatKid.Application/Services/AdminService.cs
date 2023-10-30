using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.Admin;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdminViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Identity;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net;

namespace ChatKid.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(IAdminRepository adminRepository, IMapper mapper
            , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<CommandResult> Create(AdminCreateRequest model)
        {
            model.Gmail = model.Gmail.ToLower();
            var entity = _mapper.Map<Admin>(model);

            var duplicated = await _adminRepository.TableNoTracking
                .SingleOrDefaultAsync(x => (x.Gmail == model.Gmail) || (x.Phone == model.Phone));

            if (duplicated is not null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,

                Description = string.Format(CommandMessages.Duplicated, model.Gmail + " or " +  model.Phone)
            }); 

            //--------------------------------------------------
            // add application user
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Gmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Gmail
            };
            var resultApp = await _userManager.CreateAsync(user);
            //--------------------------------------------------

            //--------------------------------------------------
            // Create new role if not exist
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            //--------------------------------------------------

            //--------------------------------------------------
            // Create admin

            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            entity.ApplicationUserId = user.Id;
            entity.Status = 1;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var result = await _adminRepository.InsertAsync(entity);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.CreateFailed, entity.Gmail)
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteAsync(Guid id)
        {
            var admin = await _adminRepository.TableNoTracking
                .Where(a => a.Status == 1 && id.Equals(a.Id)).SingleOrDefaultAsync();

            if (admin is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            }); ;

            admin.Status = 0;

            bool response = await _adminRepository.UpdateAsync(admin);
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteByEmail(string email)
        {
            if (email is null) email = "";
            var admin = await _adminRepository.TableNoTracking
                .Where(a => a.Status == 1 && email.Equals(a.Gmail)).SingleOrDefaultAsync();
            if (admin is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(CommandMessages.DeleteFailed, email)
            }); ;

            admin.Status = 0;

            bool response = await _adminRepository.UpdateAsync(admin);
            return CommandResult.SuccessWithData(email);
        }

        public async Task<AdminViewModel> GetAdminInfo(string? email, string? avatar)
        {
            var admin = await _adminRepository.TableNoTracking
                            .SingleOrDefaultAsync(a => a.Gmail.Equals(email));
            if (admin is null) return null;

            if (admin.AvatarUrl is null || admin.AvatarUrl.Length == 0)
            {
                admin.AvatarUrl = avatar;
                bool ok = await _adminRepository.UpdateAsync(admin);
            }

            var response = _mapper.Map<AdminViewModel>(admin);  

            if (response.Gmail == "kidtalkie@gmail.com")
            {
                response.Role = "SUPER_ADMIN";
            }
            else response.Role = "ADMIN";

            return response;
        }

        public async Task<List<AdminViewModel>> GetAllAdmin()
        {
            var listAdmin = await _adminRepository.TableNoTracking
                .Where(a => a.Gmail != "kidtalkie@gmail.com").ToListAsync();
            var listAdminViewModel = _mapper.Map<List<AdminViewModel>>(listAdmin);
            return listAdminViewModel;
        }

        public async Task<AdminViewModel> GetByEmail(string email)
        {
            var admin = await _adminRepository.TableNoTracking
                            .Where(a => a.Gmail.Equals(email)).SingleOrDefaultAsync();

            //if (admin is null) return null;

            var response = _mapper.Map<AdminViewModel>(admin);
            if (response.Gmail == "kidtalkie@gmail.com")
            {
                response.Role = "SUPER_ADMIN";
            }
            else response.Role = "ADMIN";
            return response;
        }

        public async Task<AdminViewModel> GetByIdAsync(Guid id)
        {
            var admin = await _adminRepository.TableNoTracking
                .SingleOrDefaultAsync(a => id.Equals(a.Id));

            var response = _mapper.Map<AdminViewModel>(admin);
            if (response.Gmail == "kidtalkie@gmail.com")
            {
                response.Role = "SUPER_ADMIN";
            }
            else response.Role = "ADMIN";
            return response;

        }

        public async Task<(int, List<AdminViewModel>)> GetPagesAsync(FilterViewModel filter
    , int pageIndex, int pageSize, string? sortBy)
        {
            if (!filter.Email.IsNullOrEmpty())
            {
                var email = filter.Email;
                /*admins = await _adminRepository.TableNoTracking
                    .SingleOrDefaultAsync(x => email.Equals(x.Gmail)) as IQueryable<Admin>;*/

                var admin = _adminRepository.TableNoTracking
                    .Where(x => email.Equals(x.Gmail));

                var resultEmail = await admin
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (admin.Count(), _mapper.Map<List<AdminViewModel>>(resultEmail));
            }

            IQueryable<Admin>? admins;

            var search = filter.SearchString;
            
            if (search.IsNullOrEmpty())
            {
                admins = _adminRepository.TableNoTracking;
            }

            else
            {
                admins = _adminRepository.TableNoTracking
                                .Where(expert => EF.Functions.ToTsVector("english", expert.FirstName + " " + expert.LastName + " " + expert.Gmail)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (expert => EF.Functions.ToTsVector("english", expert.FirstName + " " + expert.LastName + " " + expert.Gmail)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }

            admins = admins.Where(x => x.Gmail != "kidtalkie@gmail.com");

            if (filter.Status != 2) admins = admins.Where(x => x.Status == filter.Status);

            if (!sortBy.IsNullOrEmpty())
            {
                admins = admins.Sort(sortBy);
            }
            var result = await admins.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            return (admins.Count(), _mapper.Map<List<AdminViewModel>>(result));
        }

        public async Task<CommandResult> Update(Guid id, AdminUpdateRequest adminViewModel)
        {
            var existingAdmin = await _adminRepository.GetByIdAsync(id);

            if (existingAdmin is null  
                || "kidtalkie@gmail.com".Equals(existingAdmin.Gmail))

            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            }

            _mapper.Map(adminViewModel, existingAdmin);
            var result = await _adminRepository.UpdateAsync(existingAdmin);

            if (!result)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = string.Format(CommandMessages.UpdateFailed, id)
                });
            }

            return CommandResult.Success;
        }

    }
}
