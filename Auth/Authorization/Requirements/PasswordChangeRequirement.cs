using Microsoft.AspNetCore.Authorization;

namespace Auth.Authorization.Requirements;

public class PasswordChangeRequirement : IAuthorizationRequirement
{
    public PasswordChangeRequirement(bool isPasswordChangeRequired = true)
    {
        IsPasswordChangeRequired = isPasswordChangeRequired;
    }

    public bool IsPasswordChangeRequired { get; }
}