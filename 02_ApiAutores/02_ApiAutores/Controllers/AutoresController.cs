﻿using _02_ApiAutores.Entidades;
using _02_ApiAutores.Filtros;
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


        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;

        }



        //[HttpGet] // api/autores
        //public async Task<ActionResult<List<Autor>>> Get()
        //{
        //    return await context.Autores.Include(x => x.Libros).ToListAsync();
        //}

        //// Para mandar datos en nuestos endpoints ponemos entre {}
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Autor>> Get(int id)
        //{
        //    var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            
        //    if (autor == null)
        //    {
        //        return NotFound();
        //    }

        //    return autor;
        //}


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
