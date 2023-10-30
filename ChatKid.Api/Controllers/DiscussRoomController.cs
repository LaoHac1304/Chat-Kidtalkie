using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.FilterModels;
using ChatKid.Application.Models.RequestModels.DiscussRoom;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.DiscussRoomViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Extensions;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/discuss-rooms")]
    [ApiController]
    [AllowAnonymous]
    public class DiscussRoomController : ControllerBase
    {
        private readonly IDiscussRoomService _discussRoomService;
        private readonly IMapper _mapper;

        public DiscussRoomController(IDiscussRoomService discussRoomService, IMapper mapper)
        {
            _discussRoomService = discussRoomService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<DiscussRoomViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetDiscussRoomPages([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters, [FromQuery] string? sort, [FromQuery] DiscussRoomFilter roomFilter)
        {
            if (!sort.IsNullOrEmpty() && !sort.IsAcceptSort<DiscussRoomViewModel>())
                return BadRequest("Sorting field is not valid.");
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);
            filterViewModel.SearchString = SearchFilter.BuildSearchTerm(filterViewModel.SearchString);
            var (total, items) = await _discussRoomService.GetDiscussRoomPagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize, sort, roomFilter);
            return Ok(new PagedList<DiscussRoomViewModel>(items, total, parameters));
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DiscussRoomViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetDiscussRoomById(Guid id)
        {
            var discussRoom = await _discussRoomService.GetDiscussRoomByIdAsync(id);
            if (discussRoom == null) return NotFound();
            return Ok(discussRoom);
        }
        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreateDiscussRoom([FromBody] DiscussRoomCreateRequest request)
        {
            var result = await _discussRoomService.CreateDiscussRoomAsync(request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateDiscussRoom(Guid id, [FromBody] DiscussRoomUpdateRequest request)
        {
            var result = await _discussRoomService.UpdateDiscussRoomAsync(id, request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteDiscussRoom(Guid id)
        {
            var result = await _discussRoomService.DeleteDiscussRoomAsync(id);
            return StatusCode(result.GetStatusCode(), result);
        }
    }
}
