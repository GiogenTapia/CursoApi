using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace _02_ApiAutores.Utilidades
{
    public class SwaggerAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceControlador = controller.ControllerType.Namespace; // Controllers.V1
            var versionAPI = namespaceControlador.Split('.').Last().ToLower(); //v1
            controller.ApiExplorer.GroupName = versionAPI;

        }
    }
}
