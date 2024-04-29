using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs
{
    public class PeliculaCreacionDTO : PeliculaPatchDTO
    {


        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }
        public List<int> GenerosIDs { get; set; }
    }
}
