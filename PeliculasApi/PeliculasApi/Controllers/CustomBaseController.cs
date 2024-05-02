using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Helpers;

namespace PeliculasApi.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Metodo obtener lista de elementos
        /// </summary>
        /// <typeparam name="TEntidad"></typeparam>
        /// <typeparam name="TDTO"></typeparam>
        /// <returns></returns>
        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class
        {
            var entidades = await context.Set<TEntidad>().AsNoTracking().ToListAsync();
            var dto = mapper.Map<List<TDTO>>(entidades);
            return dto;

        }

        /// <summary>
        /// Metodo obtener lista elementos paginados
        /// </summary>
        /// <typeparam name="TEnditad"></typeparam>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="paginacionDTO"></param>
        /// <returns></returns>
        protected async Task<List<TDTO>> Get<TEnditad, TDTO>(PaginacionDTO paginacionDTO)
            where TEnditad: class
        {
            var queryable = context.Set<TEnditad>().AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entidades);
        }

        /// <summary>
        /// Metodo obtener un elemento
        /// </summary>
        /// <typeparam name="TEntidad"></typeparam>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IId
        {
            var entidad = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }

            return mapper.Map<TDTO>(entidad);
        }

        /// <summary>
        /// Metodo para crear elementos
        /// </summary>
        /// <typeparam name="TCreacion"></typeparam>
        /// <typeparam name="TEntidad"></typeparam>
        /// <typeparam name="TLectura"></typeparam>
        /// <param name="creacionDTO"></param>
        /// <param name="nombreRuta"></param>
        /// <returns></returns>
        protected async Task<ActionResult> Post <TCreacion, TEntidad, TLectura>
            (TCreacion creacionDTO, string nombreRuta) where TEntidad: class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var dtoLectura = mapper.Map<TLectura>(entidad);
            return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, dtoLectura);
        }

        protected async Task<ActionResult> Put<TCreacion, TEntidad>
            (int id, TCreacion creacionDTO) where TEntidad : class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<ActionResult>Patch<TEntidad,TDTO>
            (int id, JsonPatchDocument<TDTO> patchDocument) where TDTO: class
            where TEntidad : class, IId
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var entidadBD = await context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

            if (entidadBD == null) { return NotFound(); }

            var entidadDTO = mapper.Map<TDTO>(entidadBD);
            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadBD);

            await context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntidad>
            (int id) where TEntidad : class, IId, new()
        {
            var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new TEntidad()
            {
                Id = id,
            });

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
