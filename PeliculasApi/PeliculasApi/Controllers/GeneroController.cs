using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GeneroController :  ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GeneroController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await context.Genero.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidad = await context.Genero.FirstOrDefaultAsync(generobd => generobd.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<GeneroDTO>(entidad);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var generoDTO = mapper.Map<GeneroDTO>(entidad);
            return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id }, generoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO )
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await  context.Genero.AnyAsync(generobd=> generobd.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero()
            {
                Id = id,
            });

            await context.SaveChangesAsync(); 
            return NoContent();
        }
    }
}
