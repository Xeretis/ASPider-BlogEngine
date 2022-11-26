using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Auth;

public class ChangePasswordRequestModel
{
    [Required] public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}