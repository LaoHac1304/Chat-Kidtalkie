using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.KidServiceRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.KidServiceViewModel;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/kid-services")]
    [AllowAnonymous]
    [ApiController]
    public class KidServiceController : ControllerBase
    {
        private readonly IKidServiceService _kidServiceService;
        private readonly IMapper _mapper;

        public KidServiceController(IKidServiceService kidServiceService, IMapper mapper)
        {
            _kidServiceService = kidServiceService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<KidServiceViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetKidServicePages([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters)
        {
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);
            var (total, items) = await _kidServiceService.GetKidServicePagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize);
            return Ok(new PagedList<KidServiceViewModel>(items, total, parameters));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(KidServiceViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetKidServiceById([FromRoute] Guid id)
        {
            var result = await _kidServiceService.GetKidServiceAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreateKidService([FromBody] KidServiceCreateRequest request)
        {
            var result = await _kidServiceService.CreateKidServiceAsync(request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateKidService([FromRoute] Guid id, [FromBody] KidServiceUpdateRequest request)
        {
            var model = _mapper.Map<KidServiceViewModel>(request);
            var result = await _kidServiceService.UpdateKidServiceAsync(id, request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteKidService([FromRoute] Guid id)
        {
            var result = await _kidServiceService.DeleteKidServiceAsync(id);
            return StatusCode(result.GetStatusCode(), result);
        }

    }
}
