using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.NotificationRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.NotificationViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using ChatKid.PushNotification.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{

    [Route("api/notifications")]
    [ApiController]
    //[Authorize(Roles = UserRoles.Admin)]
    //[Authorize(Roles = UserRoles.Parent)]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public class NotificationController : ControllerBase
    {
        private readonly IPushNotificationService _pushNotificationService; 
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public NotificationController(
            IPushNotificationService pushNotificationService,
            INotificationService notificationService,
            IMapper mapper)
        {
            _pushNotificationService = pushNotificationService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        
        /*[HttpPost("push")]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            try
            {
                var result = await _pushNotificationService.PushNotification(notificationModel);

                if (result.IsSuccess)
                {
                    // Return 200 OK for success
                    return Ok(result);
                }
                else
                {
                    // Return 500 Internal Server Error for failures
                    return StatusCode(400, result);
                }
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with a generic error message
                return StatusCode(500, new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Internal Server Error"
                });
            }
        }*/

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Create([FromBody] NotificationCreateRequest request)
        {
            var response = await _notificationService.AddNotificationAsync(request);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Created("", response);
        }
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] NotificationUpdateRequest request)
        {
            var response = await _notificationService.UpdateNotificationAsync(id, request);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response);
        }   

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _notificationService.DeleteNotificationAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(NotificationViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetNotificationById([FromRoute] Guid id)
        {
            var response = await _notificationService.GetNotificationAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetNotificationPages([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters, [FromQuery] string? sort)
        {
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);
            var (total, items) = await _notificationService.GetNotificationPagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize, sort);
            return Ok(new PagedList<NotificationViewModel>(items, total, parameters));
        }
    }
}
