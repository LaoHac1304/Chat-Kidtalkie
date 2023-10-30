using AutoMapper;
using ChatKid.Application.Models.RequestModels.FamilyRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.IServices;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.FamilyViewModels;
using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChatKid.Api.Controllers
{
    [Route("api/families")]
    [ApiController]
    [Authorize(Roles = UserRoles.Parent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService _familyService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FamilyController(
            IFamilyService familyService, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _familyService = familyService;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FamilyViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFamilyById([FromRoute]Guid id)
        {
            var response = await _familyService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<FamilyViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] SearchFilter filter,
           [FromQuery] string? sortBy, [FromQuery] PaginationParameters parameters)
        {
            var (total, items) = await _familyService.GetPagesAsync(mapper.Map<FilterViewModel>(filter), sortBy
                , parameters.PageNumber, parameters.PageSize);
            return Ok(new PagedList<FamilyViewModel>(items, total, parameters));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody] FamilyCreateRequest request)
        {
            var email = _httpContextAccessor.HttpContext!.User.Identity!.Name;
            FamilyViewModel model = mapper.Map<FamilyViewModel>(request);
            model.OwnerMail = email;

            var response = await _familyService.CreateAsync(model);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Created("", response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] FamilyUpdateRequest request)
        {
            FamilyViewModel model = mapper.Map<FamilyViewModel>(request);
            model.Id = id;

            var response = await _familyService.UpdateAsync(model);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _familyService.DeleteAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }

        //[HttpDelete]
        //[ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        //public async Task<IActionResult> DeleteNghia()
        //{
        //    var response = await _familyService.DeleteAsyncNghia();
        //    return StatusCode(response.GetStatusCode(), response);
        //}
    }
}
