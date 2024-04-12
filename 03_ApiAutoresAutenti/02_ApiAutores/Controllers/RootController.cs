using _02_ApiAutores.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace _02_ApiAutores.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController:ControllerBase
    {
        //Prueba de hateos
        [HttpGet(Name = "ObtenerRoot")]
        public ActionResult<IEnumerable<DatoHATEOAS>> Get()
        {
            var datosHateos = new List<DatoHATEOAS>();
            //Construcción de nuestro objeto DTO
            //Estos atributos se realizan para hacer la referencia a la misma URL
            datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new {}) 
                ,descripcion: "self", "GET"));  

            return datosHateos;
        }
    }
}
