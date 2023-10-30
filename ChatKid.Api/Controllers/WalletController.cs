using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.WalletRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.WalletViewModels;
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
    [Route("api/wallets")]
    [Authorize(Roles = UserRoles.Parent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;
        private readonly IMapper _mapper;
        public WalletController(IWalletService walletService, IMapper mapper)
        {
            this.walletService = walletService;
            _mapper = mapper;
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WalletViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            WalletViewModel response = null;
            if (id != null)
            {
                response = await walletService.GetByIdAsync(id);
            }
            else
            {
                return NotFound();
            }

            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<WalletViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] SearchFilter filter,[FromQuery] string? sortBy, [FromQuery] PaginationParameters parameters)
        {
            var (total, items) = await walletService.GetPagesAsync(_mapper.Map<FilterViewModel>(filter), sortBy
                 , parameters.PageNumber, parameters.PageSize);
            return Ok(new PagedList<WalletViewModel>(items, total, parameters));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WalletCreateRequest request)
        {
            var response = await walletService.CreateAsync(_mapper.Map<WalletViewModel>(request));
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Created("", response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] WalletUpdateRequest request)
        {
            WalletViewModel model = _mapper.Map<WalletViewModel>(request);
            model.Id = id;

            var response = await walletService.UpdateAsync(model);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await walletService.DeleteAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }
    }
}
