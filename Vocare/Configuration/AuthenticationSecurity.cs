using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vocare.Model;
using Vocare.Service;
using Vocare.Service.Intefaces;

namespace Vocare.Configuration
{
    public static class AuthenticationSecurity
    {
        public static void AddAuthenticationSecurity(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.Configure<JwtSettings>(config.GetSection("JWT"));

            var settings = config.GetSection("JWT").Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(settings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                }; ;
            });
        }

        public static void UseAuthenticationSecurity(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
