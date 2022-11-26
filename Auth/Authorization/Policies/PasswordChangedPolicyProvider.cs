using Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Authorization.Policies;

public class PasswordChangedPolicyProvider : IAuthorizationPolicyProvider
{
    private const string POLICY_PREFIX = "PasswordChange";

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
            bool.TryParse(policyName.AsSpan(POLICY_PREFIX.Length), out var isPasswordChangeRequired))
        {
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            policy.AddRequirements(new PasswordChangedRequirement(isPasswordChangeRequired));
            return Task.FromResult((AuthorizationPolicy?)policy.Build());
        }

        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }
}