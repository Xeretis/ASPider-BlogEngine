namespace WebApi.Models.Auth;

public class LoginResponseModel
{
    public LoginResponseUserModel User { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class LoginResponseUserModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool ChangePassword { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool LockoutEnabled { get; set; }

    public IEnumerable<string> Roles { get; set; }
}