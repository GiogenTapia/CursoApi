using _02_ApiAutores.DTOs;
using _02_ApiAutores.Entidades;
using _02_ApiAutores.Filtros;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace _02_ApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")] // api/autores
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        //Ejemplo de obtener datos de nuestro appsettings
        [HttpGet("configuraciones")]
        public ActionResult<string> ObtenerApellido()
        {
            //return configuration["apellido"];
            return configuration["persona:nombre"];
        }



        [HttpGet] // api/autores
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            //Se coloca en un listado con el ToListAsync
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        // Para mandar datos en nuestos endpoints ponemos entre {}
        //Se ´le puede asignar un nombre para utilizarlo en otra parte
        [HttpGet("{id:int}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTOLibros>> Get(int id)
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

            return mapper.Map<AutorDTOLibros>(autor);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            //Se obtiene un solo dato a buscar con el nombre recibido
            //var autor = await context.Autores.FirstOrDefaultAsync(autorBD => autorBD.Nombre.Contains(nombre));
            
            //Se obtiene todos los usuarios que coincidan con ese nombre
            //Aqui se utilizan clausulas de filtros.
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();


            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost]
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


        [HttpPut("{id:int}")] // api/autores/1
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
