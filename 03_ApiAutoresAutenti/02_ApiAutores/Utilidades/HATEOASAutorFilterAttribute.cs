using _02_ApiAutores.DTOs;
using _02_ApiAutores.Filtros;
using _02_ApiAutores.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _02_ApiAutores.Utilidades
{
    public class HATEOASAutorFilterAttribute: HATEOASFiltroAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HATEOASAutorFilterAttribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var debeIncluir = DebeIncluirHATEOAS(context);
            
            if (!debeIncluir)
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;
            var modelo = resultado.Value as AutorDTO ?? throw new
                ArgumentException("Se esperaba una instancia de AutorDTO");

            await generadorEnlaces.GenerarEnlaces(modelo);
            await next();


        }
    }
}
