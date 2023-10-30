using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.ServiceRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Application.Models.ViewModels.ServiceViewModel;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Application.Models.ViewModels.ServiceViewModel;

namespace ChatKid.Api.Controllers
{
    [Route("api/services")]
    [ApiController]
    [AllowAnonymous]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IMapper _mapper;

        public ServiceController(IServiceService serviceService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _serviceService = serviceService;
            _httpContextAccesor = httpContextAccessor;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ServiceViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetServicePages([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters, string? sort)
        {
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);
            var (total, items) = await _serviceService.GetServicePagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize, sort);
            return Ok(new PagedList<ServiceViewModel>(items, total, parameters));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(QuestionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetService([FromRoute] Guid id)
        {
            var result = await _serviceService.GetServiceAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddService([FromBody] ServiceCreateRequest request)
        { 
            var result = await _serviceService.AddServiceAsync(request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateService([FromRoute] Guid id, [FromBody] ServiceUpdateRequest request)
        {
            var result = await _serviceService.UpdateServiceAsync(id, request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteService([FromRoute] Guid id)
        {
            var result = await _serviceService.DeleteServiceAsync(id);
            return StatusCode(result.GetStatusCode(), result);
        }


    }
}
