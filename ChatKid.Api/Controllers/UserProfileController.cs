using AutoMapper;
using ChatKid.Api.Services.Validators.ProfileUserValidator;
using ChatKid.ApiFramework.AuthTokenIssuer;
using ChatKid.Application.IServices;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.Application.Models.ViewModels.UserViewModels;
using ChatKid.Application.Models;
using ChatKid.Application.Models.RequestModels.UserProfileRequests;
using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChatKid.Api.Controllers
{
    [Route("api/profiles")]
    [ApiController]
    [Authorize(Roles = UserRoles.Parent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserProfileController : ControllerBase
    {
        private readonly ITokenIssuer _tokenIssuer;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserProfileController (ITokenIssuer tokenIssuer, IUserService userService, IMapper mapper)
        {
            _tokenIssuer = tokenIssuer;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CurrentUser), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProfileUsers([FromRoute] Guid id, [FromQuery] string? password)
        {
            var response = await _userService.UserLogin(id, password ?? string.Empty);
            if(response is null) return NotFound("User Password Invalid");

            CurrentUser user = new()
            {
                Name = response.Name,
                AvatarUrl = response.AvatarUrl,
                FamilyId = response.FamilyId,
                Status = response.Status,
                CurrentUserId = response.Id.ToString().EncryptAes(),
            };
            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserFamilyViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProfileUsers([FromQuery] Guid familyId)
        {
            var response = await _userService.GetAllAsync(familyId);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProfile([FromBody] UserProfileCreateRequests request)
        {
            var validator = new UserProfileCreateValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }
            var response = await _userService.CreateAsync(_mapper.Map<UserViewModel>(request));
            if (!response.Succeeded) StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response.GetData());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid id, [FromBody] UserProfileUpdateRequests request)
        {
            UserViewModel model = _mapper.Map<UserViewModel>(request);
            model.Id = id;

            var response = await _userService.UpdateAsync(model);
            if (!response.Succeeded) StatusCode(response.GetStatusCode(), response.GetData());
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProfile([FromRoute] Guid id)
        {
            var response = await _userService.DeleteAsync(id);
            if (!response.Succeeded) StatusCode(response.GetStatusCode(), response.GetData());
            return StatusCode(response.GetStatusCode(), response);
        }
    }
}
