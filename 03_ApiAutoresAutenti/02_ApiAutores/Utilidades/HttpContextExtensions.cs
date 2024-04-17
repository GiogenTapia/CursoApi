using Microsoft.EntityFrameworkCore;

namespace _02_ApiAutores.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            //contar el cantidad de la tabla
            double cantidad = await queryable.CountAsync();
            //Agregar en la cantidad de las respuestas
            httpContext.Response.Headers.Add("cantidadTotalRegistros",cantidad.ToString());
        }
    }
}
