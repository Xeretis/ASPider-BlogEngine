using System.Text;
using Auth.Authorization;
using Auth.Authorization.Policies;
using Auth.Authorization.Requirements;
using Auth.Services.Implementations;
using Auth.Services.Types;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contexts;

namespace Auth;

public static class InjectDependencies
{
    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });

        var core = services.AddIdentityCore<ApiUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });

        var builder = new IdentityBuilder(core.UserType, typeof(IdentityRole), services);
        builder.AddEntityFrameworkStores<ApplicationDbContext>().AddRoles<IdentityRole>()
            .AddSignInManager<SignInManager<ApiUser>>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAuthService, AuthService>();

        services.AddTransient<IPolicyEvaluator, ChallengeUnauthenticatedPolicyEvaluator>();
        services.AddTransient<PolicyEvaluator>();

        services.AddTransient<IAuthorizationHandler, PasswordChangedRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PasswordChangedPolicyProvider>();

        return services;
    }
}