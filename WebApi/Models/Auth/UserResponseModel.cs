namespace WebApi.Models.Auth;

public class UserResponseModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool ChangePassword { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool LockoutEnabled { get; set; }
}