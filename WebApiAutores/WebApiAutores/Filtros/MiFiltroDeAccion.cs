using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        // Se ejecuta antes de ejecutar la acción
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Antes de ejecutar la acción");
        }

        // Se ejecuta cuando la acción ya se ha ejecutado
        // Esto se refiere cuando se realiza una peticion
        //Se ejecuta cuando esa peticion de endpoint termino.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Después de ejecutar la acción");
        }
    }
}
