using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Data.Entities;

public class ApiUser : IdentityUser
{
    [Required] [MaxLength(128)] public string Name { get; set; }

    public string? ProfilePictureUrl { get; set; }
    public bool? ChangePassword { get; set; }

    public List<Post> Posts { get; set; }
    public List<Page> Pages { get; set; }
}