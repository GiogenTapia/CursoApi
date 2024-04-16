using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace _02_ApiAutores.Utilidades
{
    public class AgregarParametroXVersion: IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (operation == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-version",
                In = ParameterLocation.Header,
                Required = true,
            });
        }
    }
}
