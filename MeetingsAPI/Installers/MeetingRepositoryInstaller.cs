using MeetingsAPI.Repositories;
using MeetingsDomain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingsAPI.Installers
{
    /// <summary>
    /// DI Installer for the MeetingRepositry
    /// </summary>
    public class MeetingRepositoryInstaller : IInstaller
    { /// <summary>
      /// Method adds the MeetingRepositry to the services collection
      /// </summary>
      /// <param name="services">The output collection for new services</param>
      /// <param name="configuration">Configuration object of the app Startup</param>

        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMeetingRepository, MeetingRepository>();
        }
    }
}
