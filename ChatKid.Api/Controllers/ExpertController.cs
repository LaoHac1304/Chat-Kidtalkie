using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.ExpertRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ExpertViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using ChatKid.RedisService.RedisCaching;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/expert")]
    [ApiController]
    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpertController : ControllerBase
    {
        private readonly IExpertService _expertService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ExpertController(IExpertService expertService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _expertService = expertService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ExpertViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetExpertPages([FromQuery] SearchFilter filter
            , [FromQuery] PaginationParameters parameters, [FromQuery] string? sort)
        {
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);
            filterViewModel.SearchString = SearchFilter.BuildSearchTerm(filter.SearchString);
            var (total, items) = await _expertService.GetExpertPagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize, sort);
            return Ok(new PagedList<ExpertViewModel>(items, total, parameters));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ExpertViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetExpertById([FromRoute] Guid id/*, [FromQuery] string email*/)
        {
            
                var result = await _expertService.GetExpertByIdAsync(id);
                return Ok(result);
            
            /*if (email != null)
            {
                var result = await _expertService.GetExpertByEmailAsync(email);
                return Ok(result);
            }*/
            /*if (_httpContextAccessor.HttpContext is null ||
                 _httpContextAccessor.HttpContext.User is null)
                return Unauthorized();
            var emailIdentity = _httpContextAccessor.HttpContext.User.Identity!.Name;
            if (emailIdentity.IsNullOrEmpty()) 
                return Unauthorized();
            var response = await _expertService.GetExpertByEmailAsync(emailIdentity);
            return Ok(response);*/

        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddExpert([FromBody] ExpertCreateRequest request)
        {
            var response = await _expertService.AddExpertAsync(request);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateExpert([FromRoute] Guid id, [FromBody] ExpertUpdateRequest request)
        {
            var response = await _expertService.UpdateExpertAsync(id, request);
            return StatusCode(response.GetStatusCode() ,response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _expertService.DeleteExpertAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }
    }
}
