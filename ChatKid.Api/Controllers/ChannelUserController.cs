using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.ChannelUserRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelUserViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.ChannelUserViewModels;

namespace ChatKid.Api.Controllers
{
    [Route("api/channel-users")]
    [ApiController]
    [AllowAnonymous]
    public class ChannelUserController : ControllerBase
    {
        private readonly IChannelUserService _channelUserService;
        private readonly IMapper _mapper;

        public ChannelUserController(IMapper mapper
            , IChannelUserService channelUserService)
        {
            _mapper = mapper;
            _channelUserService = channelUserService;
        }

        /// <summary>
        /// Get a paged list of channel users based on filtering and pagination parameters.
        /// </summary>
        /// <param name="filter">Filter parameters to apply to the query.</param>
        /// <param name="parameters">Pagination parameters for controlling the page and page size.</param>
        /// <returns>
        /// A paged list of ChannelUserViewModel objects.
        /// </returns>

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ChannelUserViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllChannelUser([FromQuery] SearchFilter filter
            , [FromQuery] PaginationParameters parameters)
        {
            var (total, items) = await _channelUserService.GetAllChannelUser(
                _mapper.Map<FilterViewModel>(filter)
                , parameters.PageNumber, parameters.PageSize);
            return Ok(new PagedList<ChannelUserViewModel>(items, total, parameters));
        }

        /// <summary>
        /// Get a channel user by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the channel user to retrieve.</param>
        /// <returns>
        /// A ChannelUserViewModel object if found, or a 404 Not Found response if not found.
        /// </returns>

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ChannelUserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetChannelUserById([FromRoute] Guid id)
        {
            var response = await _channelUserService.GetChannelUserById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Create a new channel user based on the provided data.
        /// </summary>
        /// <param name="request">Data for creating a new channel user (ChannelUserRequest).</param>
        /// <returns>
        /// A CommandResult object with an HTTP status code indicating success or failure.
        /// </returns>

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateChannelUser(
            [FromBody] ChannelUserRequest request)
        {
            var model = _mapper
                .Map<ChannelUserViewModel>(request);

            var response = await _channelUserService.CreateChannelUser(model);
            return StatusCode(response.GetStatusCode(), response);
        }

        /// <summary>
        /// Delete a channel user by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the channel user to delete.</param>
        /// <returns>
        /// A CommandResult object with an HTTP status code indicating success or failure.
        /// If the channel user doesn't exist, it returns a 404 Not Found response.
        /// </returns>

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteChannelUser([FromRoute] Guid id)
        {
            var response = await _channelUserService.DeleteChannelUser(id);
            return StatusCode(response.GetStatusCode(), response);

        }

        /// <summary>
        /// Update an existing channel user by their unique identifier (ID) with the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the channel user to update.</param>
        /// <param name="request">Data for updating the channel user (ChannelUserRequest).</param>
        /// <returns>
        /// An HTTP response with the updated data or an error response.
        /// </returns>

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateChannelUserById(ChannelUserRequest request) 
        {
            var model = _mapper.Map<ChannelUserViewModel>(request);
            var response = await _channelUserService.UpdateById(model);
            return Ok(response);
        }
    }
}
