using MeetingsAPI.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeetingsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //сервіс підключення до бази даних 
            services.AddDbContext<MeetingsDBContext>(options =>
                            options.UseLazyLoadingProxies(false).UseSqlServer(
                                Configuration.GetConnectionString("DefaultConnection"))); //appsettings.json файл конфігурації 
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    )
                .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });
            services.AddHttpClient();
            DIInstaller.InstallServicesInAssembly(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Migrate(app);

        }

        private static void Migrate(IApplicationBuilder app)
        {

            using (var migrationSvcScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            { using (var context = migrationSvcScope.ServiceProvider.GetService<MeetingsDBContext>())
                {
                    context.Database.Migrate();
                } 
            }


        }

    }

   
}
