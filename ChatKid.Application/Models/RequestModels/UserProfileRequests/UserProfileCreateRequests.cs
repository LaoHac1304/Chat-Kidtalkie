namespace ChatKid.Application.Models.RequestModels.UserProfileRequests
{
    public record UserProfileCreateRequests
    (
        string? AvatarUrl, 
        string? Name, 
        string? Password, 
        string? Role, 
        Guid? FamilyId, 
        string? DeviceToken,
        string? Gender
    );
}
