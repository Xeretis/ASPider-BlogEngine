using Microsoft.AspNetCore.Identity;

namespace Domain.Data.Entities;

public class ApiUser : IdentityUser
{
    public string Name { get; set; }
}