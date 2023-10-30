using ChatKid.Common.CommandResult;
using OpenAI_API.Chat;

namespace ChatKid.OpenAI.ChatCompletions
{
    public interface IChatCompletionService
    {
        public Task<string> GetChatCompletion(string message);
        public Task<CommandResult> GetChatStreamCompletion(string message);
    }
}
