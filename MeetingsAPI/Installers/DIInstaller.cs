using MeetingsAPI.Installers.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MeetingsAPI.Installers
{
    /// <summary>
    /// Global DI services container for installing all the dependencies
    /// </summary>
    public static class DIInstaller
    {
        /// <summary>
        /// The method add all the services implementing the IInstaller interface
        /// </summary>
        /// <param name="services">The output collection for new services</param>
        /// <param name="configuration">Configuration object of the app Startup</param>
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var passwordSaltConfiguration = configuration.GetSection("PasswordConfiguration");
                services.Configure<PasswordConfiguration>(passwordSaltConfiguration);

            var jwtConfiguration = configuration.GetSection("JWTConfiguration");
                services.Configure<JWTConfiguration>(jwtConfiguration);

            var installers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
