namespace WebApi.Models.Users;

public class UsersIndexResponseModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool LockoutEnabled { get; set; }
    public IEnumerable<string> Roles { get; set; }
}