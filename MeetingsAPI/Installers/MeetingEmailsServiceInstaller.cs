using MeetingsAPI.Services;
using MeetingsDomain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingsAPI.Installers
{
    public class MeetingEmailsServiceInstaller : IInstaller
    {
        /// <summary>
        /// Method adds the MeetingEmailsService to the services collection
        /// </summary>
        /// <param name="services">The output collection for new services</param>
        /// <param name="configuration">Configuration object of the app Startup</param>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMeetingEmailsService, MeetingEmailsService>();
        }
    }
}
