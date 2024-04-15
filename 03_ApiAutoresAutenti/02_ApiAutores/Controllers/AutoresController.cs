using _02_ApiAutores.DTOs;
using _02_ApiAutores.Entidades;
using _02_ApiAutores.Filtros;
using _02_ApiAutores.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;


namespace _02_ApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")] // api/autores
    //Agregando autorizacion y la politica que solo lo pueda usar el claim de EsAdmin
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context
            ,IMapper mapper
            ,IConfiguration configuration
            ,IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }




        [HttpGet(Name = "obtenerAutores")] // api/autores
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //Protegiendo la autorización
        [AllowAnonymous] //aqui permitimos anonimos, para que usuarios no autenticados puedan consumirla
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromHeader] string  incluirHATEOAS)
        {
            //Se coloca en un listado con el ToListAsync
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);

        }

        // Para mandar datos en nuestos endpoints ponemos entre {}
        //Se ´le puede asignar un nombre para utilizarlo en otra parte
        [HttpGet("{id:int}", Name = "obtenerAutor")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOLibros>> Get(int id, [FromHeader] string incluirHATEOAS)
        {
            //Obtener un solo registro, se buscara por su Id
            var autor = await context.Autores
                .Include(autorBD => autorBD.AutoresLibros)
                .ThenInclude(autorlibroBD => autorlibroBD.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            //Agregando enlaces
            var dto = mapper.Map<AutorDTOLibros>(autor);

            return dto;
        }



        [HttpGet("{nombre}", Name = "obtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> GetPorNombre([FromRoute] string nombre)
        {
            //Se obtiene un solo dato a buscar con el nombre recibido
            //var autor = await context.Autores.FirstOrDefaultAsync(autorBD => autorBD.Nombre.Contains(nombre));
            
            //Se obtiene todos los usuarios que coincidan con ese nombre
            //Aqui se utilizan clausulas de filtros.
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();


            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost(Name = "crearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutorNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (existeAutorNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            //Mapeamos nuestra entidad con el valor a recibir
            var autor = mapper.Map<Autor>(autorCreacionDTO);


            //Con la funcion add se marca el dato en la base de datos
            context.Add(autor);
            //Los cambios marcados se registran en la base de datos
            await context.SaveChangesAsync();
            
            var autorDTO = mapper.Map<AutorDTO>(autor);

            //construir URL donde se encuentra el recurso
            //primero se le da el nombre a nuestro endpoint, aqui se le hizo al get con id
            //despues se checa lo que recibe y eso lo vamos a enviar
            //y por ultimo el valor a mostrar
            return CreatedAtRoute("obtenerAutor",new { id = autor.Id}, autorDTO);
        }


        [HttpPut("{id:int}", Name = "actualizarAutor")] // api/autores/1
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        } 

        [HttpDelete("{id:int}", Name = "eliminarAutor")] // api/autores/1
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
