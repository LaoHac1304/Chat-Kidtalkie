using ChatKid.Application.Models.RequestModels;
using ChatKid.Common.CommandResult;
using ChatKid.DataLayer.Identity;
using ChatKid.OpenAI.ChatCompletions;
using ChatKid.OpenAI.ImageGeneration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/gpt")]
    [ApiController]
    [Authorize(Roles = UserRoles.Parent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GPTController : ControllerBase
    {
        private readonly IChatCompletionService chatCompletionService;
        private readonly IImageGenerateService imageGenerateService;
        public GPTController(IChatCompletionService chatCompletionService,
            IImageGenerateService imageGenerateService)
        {
            this.chatCompletionService = chatCompletionService;
            this.imageGenerateService = imageGenerateService;
        }

        [Authorize(Roles = UserRoles.Parent)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("chat")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChatGpt([FromBody] BotChatCompletionRequest request)
        {
            var response = await this.chatCompletionService.GetChatCompletion(request.Message);
            //if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            if (response.IsNullOrEmpty()) return NotFound();
            return Ok(response);
        }

        [Authorize(Roles = UserRoles.Parent)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("chat-stream")]
        [ProducesResponseType(typeof(IAsyncEnumerable<ChatResult>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChatStreamGpt([FromBody] BotChatCompletionRequest request)
        {
            var response = await this.chatCompletionService.GetChatStreamCompletion(request.Message);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response.GetData());
        }

        [HttpPost("image")]
        [ProducesResponseType(typeof(ImageResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateImage([FromBody] BotImageGenerateRequest request)
        {
            var response = await this.imageGenerateService.GenerateImage(request.Promt, request.Quantity, request.Size);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response.GetData());
        }
    }
}
