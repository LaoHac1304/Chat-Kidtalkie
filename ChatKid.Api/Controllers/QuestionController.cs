using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.QuestionRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;

namespace ChatKid.Api.Controllers
{
    [Route("api/questions")]
    [ApiController]
    [AllowAnonymous]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<QuestionViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetQuestionPages([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters, string? sort)
        {
            var filterViewModel = _mapper.Map<FilterViewModel>(filter); 
            var (total, items) = await _questionService.GetQuestionPagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize, sort);
            return Ok(new PagedList<QuestionViewModel>(items, total, parameters));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(QuestionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            var result = await _questionService.GetQuestionByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionCreateRequest request)
        { 
            var result = await _questionService.CreateQuestionAsync(request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] QuestionUpdateRequest request)
        {
            var result = await _questionService.UpdateQuestionAsync(id, request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);
            return StatusCode(result.GetStatusCode(), result);
        }
    }
}
