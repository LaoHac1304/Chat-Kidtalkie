using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.ChannelRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/channels")]
    [ApiController]
    [AllowAnonymous]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _channelService;
        private readonly IMapper _mapper;

        public ChannelController(IChannelService channelService, IMapper mapper)
        {
            _channelService = channelService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a paged list of channels based on filtering and pagination parameters.
        /// </summary>
        /// <returns>A paged list of ChannelViewModel objects.</returns>

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ChannelViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllChannel([FromQuery] SearchFilter filter
            , [FromQuery] PaginationParameters parameters)
        {
            var (total, items) = await _channelService.GetAllChannel(_mapper.Map<FilterViewModel>(filter)
                , parameters.PageNumber, parameters.PageSize);
            return Ok(new PagedList<ChannelViewModel>(items, total, parameters));
        }

        /// <summary>
        /// Get a channel by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the channel to retrieve.</param>
        /// <returns>A ChannelViewModel object if found, or a 404 Not Found response if not found.</returns>

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ChannelViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetChannelById([FromRoute] Guid id)
        {
            var response = await _channelService.GetChannelById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Create a new channel based on the provided data.
        /// </summary>
        /// <param name="request">Data for creating a new channel (ChannelCreateRequest).</param>
        /// <returns>A CommandResult object with an HTTP status code indicating success or failure.</returns>

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateChannel(
            [FromBody] ChannelCreateRequest request)
        {
            var model = _mapper
                .Map<ChannelCreateViewModel>(request);

            var response = await _channelService.CreateChannel(model);
            return StatusCode(response.GetStatusCode(), response);
        }

        /// <summary>
        /// Delete a channel by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the channel to delete.</param>
        /// <returns>A CommandResult object with an HTTP status code indicating success or failure.
        /// If the channel doesn't exist, it returns a 404 Not Found response.</returns>
        /// 
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteChannel([FromRoute] Guid id)
        {
            var response = await _channelService.DeleteChannel(id);
            return StatusCode(response.GetStatusCode(), response);

        }

        /// <summary>
/// Update an existing channel using the provided data.
/// </summary>
/// <param name="id">The unique identifier of the channel to update.</param>
/// <param name="request">Data for updating the channel (ChannelUpdateRequest).</param>
/// <returns>A CommandResult object with an HTTP status code indicating success or failure.</returns>

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateChannel([FromRoute] Guid id,
            [FromBody] ChannelUpdateRequest request)
        {
            var model = _mapper
                .Map<ChannelViewModel>(request);
            model.Id = id;

            var response = await _channelService.UpdateChannel(model);
            return StatusCode(response.GetStatusCode(), response);
        }

       /* [HttpPost("add-users")]
        public async Task<IActionResult> AddUsers([FromBody] AddUsersRequest request)
        {
            var model = _mapper.Map<AddUsersViewModel>(request);
            var response = await _channelService.AddUsers(model);
            return StatusCode(response.GetStatusCode());
        }*/
    }
}
