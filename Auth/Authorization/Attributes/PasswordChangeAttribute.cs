using Microsoft.AspNetCore.Authorization;

namespace Auth.Authorization.Attributes;

public class PasswordChangeAttribute : AuthorizeAttribute
{
    private const string POLICY_PREFIX = "PasswordChange";

    public PasswordChangeAttribute(bool isPasswordChangeRequired)
    {
        IsPasswordChangeRequired = isPasswordChangeRequired;
    }

    public bool IsPasswordChangeRequired
    {
        get
        {
            if (bool.TryParse(Policy?.Substring(POLICY_PREFIX.Length), out var isPasswordChangeRequired))
                return isPasswordChangeRequired;
            return true;
        }
        set => Policy = $"{POLICY_PREFIX}{value.ToString()}";
    }
}