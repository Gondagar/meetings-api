using AutoMapper;
using MeetingsAPI.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingsAPI.Installers
{
    /// <summary>
    /// DI Installer for the AutoMapper
    /// </summary>
    public class AutoMapperInstaller : IInstaller
    {
        /// <summary>
        /// Method adds the AutoMapper to the services collection
        /// </summary>
        /// <param name="services">The output collection for new services</param>
        /// <param name="configuration">Configuration object of the app Startup</param>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<UserProfile>(); 
                cfg.AddProfile<MeetingProfile>(); 
            });

            mapperConfiguration.CompileMappings();
            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
