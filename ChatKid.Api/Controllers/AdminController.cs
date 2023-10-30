using AutoMapper;
using ChatKid.ApiFramework.AuthTokenIssuer;
using ChatKid.Application.IServices;
using ChatKid.Application.Models;
using ChatKid.Application.Models.RequestModels.Admin;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdminViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Constants;
using ChatKid.Common.Extensions;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChatKid.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenIssuer _tokenIssuer;

        public AdminController(IAdminService adminService, IMapper mapper
            , IHttpContextAccessor httpContextAccessor, ITokenIssuer tokenIssuer)
        {
            _adminService = adminService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenIssuer = tokenIssuer;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AdminCreateRequest request)
        {
            var response = await _adminService.Create(request);
            return StatusCode(response.GetStatusCode(), response.GetData());
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AdminViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            AdminViewModel response = null;
            
            response = await _adminService.GetByIdAsync(id);
            return Ok(response);
            
            /*else
            {
                var currentUser = GetInfoCurrentUer();
                if (currentUser is null) return Unauthorized();

                var currentEmail = currentUser.Email;
                var currentAvatar = currentUser.Avatar;

                response = await _adminService.GetAdminInfo(currentEmail, currentAvatar);
                
            }
            if (response is null) NotFound();
            return Ok(response);*/
        }

        private CurrentUserInfoGoogle? GetInfoCurrentUer()
        {
            if (_httpContextAccessor.HttpContext is null ||
                _httpContextAccessor.HttpContext.User is null)
                return null;

            var token = _httpContextAccessor.HttpContext!
                .Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var principal = _tokenIssuer.GetClaimsPrincipal(token);

            var avatar_url = principal.Claims
                .SingleOrDefault(x => x.Type.Equals(ClaimTypesUrlConstant.AvatarUrl));

            var avatar = avatar_url.ToString();
            avatar = avatar.Replace("avatar_url:", "");

            avatar = avatar.Replace(" ", "");

            var email = _httpContextAccessor.HttpContext.User.Identity!.Name;

            var currentUserInfoGoogle = new CurrentUserInfoGoogle
            {
                Email = email,
                Avatar = avatar
            };

            return currentUserInfoGoogle;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<AdminViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPagination([FromQuery] SearchFilter? filter
            , [FromQuery] PaginationParameters parameters, [FromQuery] string? sort)
        {
            if (!sort.IsNullOrEmpty() && !sort.IsAcceptSort<AdminViewModel>())
                return BadRequest("Sorting field is not valid.");
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);

            filterViewModel.SearchString
                = SearchFilter.BuildSearchTerm(filterViewModel.SearchString);

            var (total, items) = await _adminService.GetPagesAsync(filterViewModel
                , parameters.PageNumber, parameters.PageSize, sort);
            return Ok(new PagedList<AdminViewModel>(items, total, parameters));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var response = await _adminService.DeleteAsync(id);
            return StatusCode(response.GetStatusCode(),response.GetData());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AdminUpdateRequest request)
        {
            var response = await _adminService.Update(id, request);
            return StatusCode(response.GetStatusCode(), response.GetData());
        }

    }
}
