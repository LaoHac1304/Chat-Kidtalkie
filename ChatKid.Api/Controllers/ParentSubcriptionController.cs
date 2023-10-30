using ChatKid.Application.Models.SearchFilter;
using ChatKid.Common.Pagination;
using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatKid.Api.Controllers
{
    [ApiController]
    [Route("api/parent-subcriptions")]
    [Authorize(Roles = UserRoles.Parent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParentSubcriptionController : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchFilter filter, [FromQuery] PaginationParameters parameters)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Created("", null);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update()
        {
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
