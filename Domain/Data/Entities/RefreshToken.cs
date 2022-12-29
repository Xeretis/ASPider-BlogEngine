using Domain.Common;

namespace Domain.Data.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }

    public string UserId { get; set; }
    public ApiUser? User { get; set; }
}