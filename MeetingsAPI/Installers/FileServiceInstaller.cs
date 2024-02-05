using MeetingsAPI.Services;
using MeetingsDomain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingsAPI.Installers
{
    /// <summary>
    /// DI Installer for the FileService
    /// </summary>
    public class FileServiceInstaller : IInstaller
    {
        /// <summary>
        /// Method adds the FileService to the services collection
        /// </summary>
        /// <param name="services">The output collection for new services</param>
        /// <param name="configuration">Configuration object of the app Startup</param>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileService, FileService>();
        }
    }
}
