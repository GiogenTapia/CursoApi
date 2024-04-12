using _02_ApiAutores.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _02_ApiAutores.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }


        //Creación de hateos
        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            var datosHateos = new List<DatoHATEOAS>();

            //obtener si es admin
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            //Construcción de nuestro objeto DTO
            //Estos atributos se realizan para hacer la referencia a la misma URL
            datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { })
                , descripcion: "self", "GET"));
            datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new { }),
                descripcion: "autores", metodo: "GET"));

            //validar si es admin
            if (esAdmin.Succeeded)
            {
                datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("crearAutores", new { }),
                    descripcion: "autor-crear", metodo: "POST"));

                datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("crearLibro", new { }),
                    descripcion: "libro-crear", metodo: "POST"));
            }



            return datosHateos;
        }
    }
}
