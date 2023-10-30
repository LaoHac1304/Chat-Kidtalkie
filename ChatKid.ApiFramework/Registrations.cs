using ChatKid.ApiFramework.Authentication;
using ChatKid.ApiFramework.AuthJwtIssuer;
using ChatKid.DataLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatKid.ApiFramework
{
    public static class Registrations
    {
        public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection(AuthenticationSettings.AppSettingsSection).Get<AuthenticationSettings>();
            services.AddSingleton(_ => authenticationSettings);

            var trustedIssuer = configuration.GetSection(TrustedIssuerSettings.AppSettingsSection).Get<TrustedIssuerSettings>();
            services.AddSingleton(_ => trustedIssuer);

            services.AddSingleton<IJwtTokenIssuer, JwtTokenIssuer>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            return services;
        }

    }
}
