using _02_ApiAutores.DTOs;
using _02_ApiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace _02_ApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("{id:int}", Name = "obtenerLibrov1")]
        public async Task<ActionResult<LibroDTOAutores>> Get(int id)
        {
            //Esto es por si queremos mandar los comentarios, pero se ignora para 
            //que el usuario no consuma muchos datos
            // var libro =  await context.Libros.Include(libroBD => libroBD.Comentarios).FirstOrDefaultAsync(x => x.Id == id);


            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOAutores>(libro);
        }

        [HttpPost(Name = "crearLibrov1")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autor/es");
            }

            var autoresIds = await context.Autores.Where(autorBD => libroCreacionDTO.AutoresIds.Contains(autorBD.Id)).
                Select(x => x.Id).ToListAsync();
            if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados.");
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);


            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDTO = mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDTO);
        }

        [HttpPut("{id:int}", Name = "actualizarLibrov1")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {

            //se guarda en memoria el registro
            //aqui lo que se hace es incluir al igual los autoreslibros
            //despues obener el libro con el mismo id a actualizar
            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            //lo reasignamos con el mapper para crear su tipo
            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        //El patch nos ayuda a actualizar datos en especifico sin poner toda la entidad
        //para ello recuerda instalar el nuget de newtonsoft y agregarlo a startup
        [HttpPatch("{id:int}", Name = "patchLibrov1")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroBD = await context.Libros.FirstOrDefaultAsync(libroBD => libroBD.Id == id);

            if (libroBD == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroBD);

            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroBD);

            await context.SaveChangesAsync();

            return NoContent();

        }


        [HttpDelete("{id:int}", Name = "borrarLibrov1")]
        public async Task<ActionResult> Detele(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Libro { Id = id });
            await context.SaveChangesAsync();
            return NoContent();

        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }
        }

    }


}
