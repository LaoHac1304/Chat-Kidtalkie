using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.BlogRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.BlogViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Extensions;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        public BlogController(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            this._mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BlogViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetBlogById([FromRoute] Guid id)
        {
            var response = await _blogService.GetBlogByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<BlogViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetBlogPages([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters, [FromQuery] string? sort)
        {
            if (!sort.IsNullOrEmpty() && !sort.IsAcceptSort<BlogViewModel>())
                return BadRequest("Sorting field is not valid.");


            var filterViewModel = _mapper.Map<FilterViewModel>(filter);

            filterViewModel.SearchString = SearchFilter.BuildSearchTerm(filterViewModel.SearchString);


            var (total, items) = await _blogService.GetBlogPagesAsync(filterViewModel, parameters.PageNumber, parameters.PageSize, sort);
            return Ok(new PagedList<BlogViewModel>(items, total, parameters));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Create([FromBody] BlogCreateRequest request)
        {
            var response = await _blogService.AddBlogAsync(request);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response);
            return Created("", response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BlogUpdateRequest request)
        {

            var response = await _blogService.UpdateBlogAsync(id, request);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _blogService.DeleteBlogAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }
        
    }
}
