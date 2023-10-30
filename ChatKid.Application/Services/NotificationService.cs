using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.NotificationRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.BlogViewModels;
using ChatKid.Application.Models.ViewModels.NotificationViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<NotificationViewModel> GetNotificationAsync(Guid id)
        {
            var notification = await _notificationRepository.TableNoTracking.Where(x => x.Id.Equals(id)).Include(x => x.CreateAdmin).FirstAsync();
            return _mapper.Map<NotificationViewModel>(notification);
        }



        public async Task<(int, List<NotificationViewModel>)> GetNotificationPagesAsync(FilterViewModel filter, int pageIndex, int pageSize, string? sort)
        {
            IQueryable<Notification> notifications;
            string search = filter.SearchString ?? "";
            if (search.IsNullOrEmpty())
            {
                notifications = _notificationRepository.TableNoTracking;
            }

            else
            {
                notifications = _notificationRepository.TableNoTracking
                                .Where(notification => EF.Functions.ToTsVector("english", "" + notification.Content)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (notification => EF.Functions.ToTsVector("english","" + notification.Content)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }
            if (filter.Status != 2) notifications = notifications.Where(x => x.Status == filter.Status);
            if (!sort.IsNullOrEmpty())
            {
                notifications = notifications.Sort(sort);
            }
            var result = await notifications.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (notifications.Count(), _mapper.Map<List<NotificationViewModel>>(result));
        }

        public async Task<CommandResult> AddNotificationAsync(NotificationCreateRequest model)
        {
            var notification = _mapper.Map<Notification>(model);
            if (notification == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = "Notification is null"
                });
            }
            if (_httpContextAccessor.HttpContext.User is null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Description = string.Format("Unauthorzied")
                });
            }

            var currentUser = await _adminRepository.GetAdminByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            if (currentUser == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, "Admin")
            });
            notification.CreatedBy = currentUser.Id;
            var result = await _notificationRepository.InsertAsync(notification);
            if (!result)
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.CreateFailed, notification.Id)
                });
            return CommandResult.Success;

        }

        public async Task<CommandResult> DeleteNotificationAsync(Guid id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            }
            notification.Status = 0;
            var result = await _notificationRepository.UpdateAsync(notification);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, notification.Id)
            });
            return CommandResult.Success;
        }
        public async Task<CommandResult> UpdateNotificationAsync(Guid id, NotificationUpdateRequest model)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            _mapper.Map(model, notification);
            var result = await _notificationRepository.UpdateAsync(notification);
            if (!result)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = string.Format(CommandMessages.UpdateFailed, id)
                });
            }
            return CommandResult.Success;
        }


    }
}
