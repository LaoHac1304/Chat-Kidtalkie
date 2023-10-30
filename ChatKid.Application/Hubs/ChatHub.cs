using ChatKid.Common.CommandResult;
using ChatKid.OpenAI.ChatCompletions;
using ChatKid.OpenAI.ImageGeneration;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatCompletionService _chatCompletionService;

        public ChatHub(IChatCompletionService chatCompletionService)
        {
            _chatCompletionService = chatCompletionService;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }

        public async Task SendMessageToGPT(string message)
        {
            var response = await _chatCompletionService.GetChatCompletion(message);

            

            await Clients.All.SendAsync("ReceiveMessage", response);
        }
    }
}
