using Microsoft.AspNetCore.Authorization;

namespace Auth.Authorization.Requirements;

public class PasswordChangedRequirement : IAuthorizationRequirement
{
    public PasswordChangedRequirement(bool isPasswordChangeRequired = true)
    {
        IsPasswordChangeRequired = isPasswordChangeRequired;
    }

    public bool IsPasswordChangeRequired { get; }
}