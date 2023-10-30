using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.MessageRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.MessageViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.MessageViewModels;

namespace ChatKid.Api.Controllers
{
    [Route("api/messages")]
    [ApiController]
    [AllowAnonymous]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessageController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new message based on the provided data.
        /// </summary>
        /// <param name="request">Data for creating a new message (MessageCreateRequest).</param>
        /// <returns>
        /// A CommandResult object with an HTTP status code indicating success or failure.
        /// </returns>

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMessage(MessageCreateRequest request)
        {
            var model = _mapper.Map<MessageViewModel>(request);
            var response = await _messageService.AddMessage(model);
            return StatusCode(response.GetStatusCode(), response);
        }

        /// <summary>
        /// Get a paged list of messages based on filtering and pagination parameters.
        /// </summary>
        /// <param name="filter">Filter parameters to apply to the query.</param>
        /// <param name="parameters">Pagination parameters for controlling the page and page size.</param>
        /// <returns>A paged list of MessageViewModel objects.</returns>

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<MessageViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllMessage([FromQuery] SearchFilter filter,
            [FromQuery] PaginationParameters parameters)
        {
            var (total, items) = await _messageService
                .GetAllMessage(_mapper.Map<FilterViewModel>(filter)
                , parameters.PageNumber
                , parameters.PageSize);

            return Ok(new PagedList<MessageViewModel>(items, total, parameters));
        }

        /// <summary>
        /// Delete a message by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the message to delete.</param>
        /// <returns>
        /// A CommandResult object with an HTTP status code indicating success or failure.
        /// </returns>

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
        {
            var response = await _messageService.DeleteMessage(id);
            return StatusCode(response.GetStatusCode(), response);
        }

        /// <summary>
        /// Get a message by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the message to retrieve.</param>
        /// <returns>
        /// A MessageViewModel object if found, or a 404 Not Found response if not found.
        /// </returns>

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MessageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMessageById([FromRoute] Guid id)
        {
            var response = await _messageService.GetMessageById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Update an existing message with the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the message to update.</param>
        /// <param name="request">Data for updating the message (MessageUpdateRequest).</param>
        /// <returns>
        /// A CommandResult object with an HTTP status code indicating success or failure.
        /// </returns>

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMessage([FromRoute] Guid id,
            [FromBody] MessageUpdateRequest request)
        {
            var model = _mapper
                .Map<MessageViewModel>(request);
            model.Id = id;

            var response = await _messageService.UpdateMessage(model);
            return StatusCode(response.GetStatusCode(), response);
        }
    }
}
