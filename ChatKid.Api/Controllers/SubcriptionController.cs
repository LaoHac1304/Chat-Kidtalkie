using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.SubcriptionRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.SubcriptionViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [ApiController]
    [Route("api/subcriptions")]
    [Authorize(Roles = UserRoles.Parent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubcriptionController : ControllerBase
    {
        private readonly ISubcriptionService subcriptionService;
        private readonly IMapper _mapper;

        public SubcriptionController(ISubcriptionService subcriptionService, IMapper mapper)
        {
            this.subcriptionService = subcriptionService;
            this._mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SubcriptionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            SubcriptionViewModel response = null;
            if(id != null)
            {
                response = await subcriptionService.GetByIdAsync(id);
            }
            else
            {
                return NotFound();
            }

            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<SubcriptionViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] SearchFilter filter, [FromQuery] string? sortBy, [FromQuery] PaginationParameters parameters)
        {
            var (total, items) = await subcriptionService.GetPagesAsync(_mapper.Map<FilterViewModel>(filter), sortBy
                , parameters.PageNumber, parameters.PageSize);
            return Ok(new PagedList<SubcriptionViewModel>(items, total, parameters));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubcriptionCreateRequest request)
        {
            var response = await subcriptionService.CreateAsync(_mapper.Map<SubcriptionViewModel>(request));
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Created("", response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SubcriptionUpdateRequest request)
        {
            SubcriptionViewModel model = _mapper.Map<SubcriptionViewModel>(request);
            model.Id = id;

            var response = await subcriptionService.UpdateAsync(model);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await subcriptionService.DeleteAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }
    }
}
