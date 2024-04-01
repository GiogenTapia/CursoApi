using _02_ApiAutores.DTOs;
using _02_ApiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _02_ApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context, 
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }
            var libros = await context.Comentarios.Where(comentarioDB => comentarioDB.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(libros);
        }


        [HttpPost]
        public async Task<ActionResult> Post(int libroId, 
            ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId= libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
