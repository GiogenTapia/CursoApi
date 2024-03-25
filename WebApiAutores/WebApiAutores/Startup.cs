using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAutores.Controllers;
using WebApiAutores.Middlewares;
using WebApiAutores.Servicios;

namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(x=>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            //Ejemplo de servicios ASP.Net Core

            services.AddTransient<IServicio, ServicioA>();
            //services.AddTransient<ServicioA>();
            //services.AddScoped<IServicio, ServicioA>();
            //services.AddSingleton<IServicio, ServicioA>();
            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }


        //Configuraciones de Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            //creada mediante clase
            app.UseLoguearRespuestaHTTP();

            app.Map("/ruta1", app =>
            {
                app.Run(async contexto =>
                {
                    await contexto.Response.WriteAsync("Estoy interceptando la tubería");

                });
            });



            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
            });

        }
    }
}
