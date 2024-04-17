using _02_ApiAutores.DTOs;

namespace _02_ApiAutores.Utilidades
{
    public static class IQuerybleExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> query, PaginacionDTO paginacionDTO)
        {
            return query
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina)
                .Take(paginacionDTO.RecordsPorPagina);
        }
    }
}
