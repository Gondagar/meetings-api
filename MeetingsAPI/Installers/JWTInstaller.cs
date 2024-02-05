using MeetingsAPI.Installers.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MeetingsAPI.Installers
{
    public class JWTInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            JWTConfiguration settings = new JWTConfiguration();

            configuration.GetSection("JWTConfiguration").Bind(settings);
            string secret = settings.Secret;
            byte[] secretByteArra = Encoding.ASCII.GetBytes(settings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretByteArra),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }
    }
}
