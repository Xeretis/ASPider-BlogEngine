using System.ComponentModel.DataAnnotations;

namespace Domain.Data.Models.Auth;

public class LoginRequestModel
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}