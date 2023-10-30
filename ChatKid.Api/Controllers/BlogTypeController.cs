using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.TypeBlogRequests;
using ChatKid.Application.Models.ViewModels.TypeBlogViewModels;
using ChatKid.Common.CommandResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/blog-types")]
    [AllowAnonymous]
    [ApiController]
    public class BlogTypeController : ControllerBase
    {
        private readonly ITypeBlogService _typeBlogService;
        private readonly IMapper _mapper;
        public BlogTypeController(ITypeBlogService typeBlogService, IMapper mapper)
        {
            _typeBlogService = typeBlogService;
            this._mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TypeBlogViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetTypeBlogById([FromRoute] Guid id)
        {
            var response = await _typeBlogService.GetTypeBlogById(id);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TypeBlogViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAllTypeBlog()
        {
            var response = await _typeBlogService.GetAllTypeBlog();
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddTypeBlog([FromBody] TypeBlogCreateRequest request)
        {
            var result = await _typeBlogService.CreateTypeBlog(request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateTypeBlog([FromRoute] Guid id, [FromBody] TypeBlogUpdateRequest request)
        {
            var result = await _typeBlogService.UpdateTypeBlog(id, request);
            return StatusCode(result.GetStatusCode(), result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteTypeBlog([FromRoute] Guid id)
        {
            var result = await _typeBlogService.DeleteTypeBlog(id);
            return StatusCode(result.GetStatusCode(), result);
        }
    }
}
