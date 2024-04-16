using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace _02_ApiAutores.Utilidades
{
    public class AgregarParametrosHATEOAS : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //Filtrar solo en los metodos GET
            if (context.ApiDescription.HttpMethod != "GET")
            {
                return;
            }

            if (operation == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "incluirHATEOAS",
                In = ParameterLocation.Header,
                Required = false,
            });
        }
    }
}
