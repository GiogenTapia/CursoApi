using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")] // api/autores
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }


        //Ejemplo de servicios ASP.Net Core

        [HttpGet("GUID")]
        //Esta respuesta se va a guardar en memoria durtante 10 segundos
        [ResponseCache(Duration = 10)]
        // Agregando nuestro filtro personalizado
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult ObtenerGuids()
        {
            return Ok(new {
                AutoresControllerTransient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                AutoresControllerScope = servicioScoped.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),
                AutoresControllerSingleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.ObtenerSingleton(),

            });

        }

        [HttpGet] // api/autores
        [HttpGet("listado")] //  api/autores/listado
        [HttpGet("/listado")]// /listado
        [Authorize] // Agregado de autorización, se puede poner a nivel global
        public async Task<ActionResult<List<Autor>>> Get()
        {
            logger.LogInformation("Estamnos obteniendo los autores");
            logger.LogWarning("Esto es un mensaje de prueba");
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        // Para mandar datos en nuestos endpoints ponemos entre {}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            
            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get([FromRoute] string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            var existeAutorNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if (existeAutorNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");
            }

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("primero")] // api/autores/primero
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor)
        {
            return await context.Autores.FirstOrDefaultAsync();
        }



        [HttpPut("{id:int}")] // api/autores/1
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        } 

        [HttpDelete("{id:int}")] // api/autores/1
        //
        public async Task<ActionResult> Detele(int id)
        {
            var existe = await context.Autores.AnyAsync(x=> x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
