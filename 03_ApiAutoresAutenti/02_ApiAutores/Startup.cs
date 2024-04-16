using _02_ApiAutores.Filtros;
using _02_ApiAutores.Middlewares;
using _02_ApiAutores.Servicios;
using _02_ApiAutores.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;


namespace _02_ApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Limpiando mapeo para obtener los datos de los claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services)
        {
            //Agregar filtro de manera global
            //Aqui se agrega el newtonsoft
            services.AddControllers(opciones =>
            {
                opciones.Conventions.Add(new SwaggerAgrupaPorVersion());
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x=>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();

            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));


            // Agregando autentificación
            //Configurando firma de token y validacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opcines => opcines.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                    ClockSkew = TimeSpan.Zero

                });

            services.AddEndpointsApiExplorer();

            //Configurar Swagger para mandar nuestro JWT
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiAutores", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiAutores", Version = "v2" });
                c.OperationFilter<AgregarParametrosHATEOAS>();
                c.OperationFilter<AgregarParametroXVersion>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }

                });
            });

            //Agregamos el Automapper
            services.AddAutoMapper(typeof(Startup));


            //Agregando el servicio de autenticación
            services.AddIdentity<IdentityUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            //Agregando politicas de autorizacion 
            services.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));

            });


            //Con esto tenemos acceso a los servicios de proteccion de datos
            services.AddDataProtection();

            //Agregando las opciones de CORS
            //Aqui ponemos el origen de nuestra url
            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("algunaURL").AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddTransient<GeneradorEnlaces>();
            services.AddTransient<HATEOASAutorFilterAttribute>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Agregando el servicio HASH
            services.AddTransient<HashService>();

        }


        //Configuraciones de Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            //creada mediante clase
            app.UseLoguearRespuestaHTTP();



            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI( c => { 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json","WebApiAutores v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutores v2");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Usar CORS en nuestra api
            app.UseCors();

            // Agregando filtro de autorización
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
            });

        }
    }
}
