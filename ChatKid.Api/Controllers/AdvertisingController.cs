using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.FilterModels;
using ChatKid.Application.Models.RequestModels.Advertising;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdvertisingViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/advertisings")]
    [ApiController]
    [AllowAnonymous]
    public class AdvertisingController : ControllerBase
    {
        private readonly IAdvertisingService _advertisingService;
        private readonly IMapper _mapper;

        public AdvertisingController(IAdvertisingService advertisingService, IMapper mapper)
        {
            _advertisingService = advertisingService;
            _mapper = mapper;
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AdvertisingDetailViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAdvertisingById([FromRoute] Guid id)
        {
            AdvertisingDetailViewModel response;

            if (!id.Equals(Guid.Empty))
            {
                response = await _advertisingService.GetAdvertisingAsync(id);
            }
            else
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<AdvertisingDetailViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAdvertisingPages([FromQuery] SearchFilter filter, [FromQuery] AdvertisingFilter advertisingFilter
            , [FromQuery] PaginationParameters parameters, [FromQuery] string? sort)
        {
            var filterViewModel = _mapper.Map<FilterViewModel>(filter);

            filterViewModel.SearchString 
                = SearchFilter.BuildSearchTerm(filterViewModel.SearchString);

            var (total, items) = await _advertisingService.GetAdvertisingPagesAsync
                (filterViewModel, advertisingFilter, parameters.PageNumber, parameters.PageSize, sort);


            return Ok(new PagedList<AdvertisingDetailViewModel>(items, total, parameters));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Create([FromBody] AdvertisingCreateRequest request)
        {
            var response = await _advertisingService.AddAdvertisingAsync(request);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Created("", response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AdvertisingUpdateRequest request)
        {

            var response = await _advertisingService.UpdateAdvertisingAsync(id, request);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpPut("clicks/{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Click([FromRoute] Guid id)
        {
            var response = await _advertisingService.ClickAdvertisingAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _advertisingService.DeleteAdvertisingAsync(id);
            return StatusCode(response.GetStatusCode(), response);
        }

        
    }
}
