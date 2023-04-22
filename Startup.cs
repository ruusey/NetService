using Microsoft.EntityFrameworkCore;
using NetService.Async;
using NetService.Repo;
using NetService.Service;

namespace NetService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EventLogRepo>(opt =>
  opt.UseInMemoryDatabase("EventLog"), ServiceLifetime.Scoped);
            services.AddTransient<EventReaderService>();
            services.AddScoped<IScopedService, ScopedServiceBase>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
           

            //services.AddControllers
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();
           // app.UseHttpsRedirection();
            app.UseStaticFiles();
           // app.useSc
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
