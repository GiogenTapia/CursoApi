using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GeneroController : CustomBaseController
    {

        public GeneroController(ApplicationDbContext context,
            IMapper mapper)
            :base(context, mapper)
        {

        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            return await Get<Genero, GeneroDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
           return await Get<Genero, GeneroDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "obtenerGenero");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO )
        {
            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
           return await Delete<Genero>(id);
        }
    }
}
