namespace ChatKid.Application.Models.RequestModels.UserProfileRequests
{
    public record UserProfileUpdateRequests
    (
        string? AvatarUrl,
        string? Name,
        string? Password,
        string? Gender,
        string? DeviceToken
    );
}
