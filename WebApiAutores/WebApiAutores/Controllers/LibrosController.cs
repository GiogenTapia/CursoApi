using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get (int id)
        {
            return await context.Libros.Include(x=> x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Libros.AnyAsync(x=> x.Id == libro.Id);

            if (!existeAutor)
            {
                return BadRequest($"No existe el autor  de ID: {libro.Id}");
            }

            context.Add(libro);
            context.SaveChangesAsync();
            return Ok();
        }

    }
}
