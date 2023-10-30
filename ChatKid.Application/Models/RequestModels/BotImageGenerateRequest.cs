namespace ChatKid.Application.Models.RequestModels
{
    public record BotImageGenerateRequest
    (
        string Promt,
        int Quantity,
        int Size
    );
    
}
