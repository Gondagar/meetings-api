using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingsAPI.Installers
{
    /// <summary>
    /// Interface which every Installer should implement
    /// </summary>
    public interface IInstaller
    {
        /// <summary>
        /// Method for adding service to the services collection
        /// </summary>
        /// <param name="services">The output collection for new services</param>
        /// <param name="configuration">Configuration object of the app Startup</param>
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
