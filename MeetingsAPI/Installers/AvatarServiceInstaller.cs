using MeetingsAPI.Services;
using MeetingsDomain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingsAPI.Installers
{
    /// <summary>
    /// DI Installer for the AvatarService
    /// </summary>
    public class AvatarServiceInstaller : IInstaller
    { 
        /// <summary>
        /// Method adds the AvatarService to the services collection
        /// </summary>
        /// <param name="services">The output collection for new services</param>
        /// <param name="configuration">Configuration object of the app Startup</param>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAvatarService, AvatarService>();
        }
    }
}
