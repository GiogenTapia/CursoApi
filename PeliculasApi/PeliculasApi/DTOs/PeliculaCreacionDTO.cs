using Microsoft.AspNetCore.Mvc;
using PeliculasApi.Helpers;
using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace PeliculasApi.DTOs
{
    public class PeliculaCreacionDTO : PeliculaPatchDTO
    {


        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIDs { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculasCreacionDTO>>))]
        public List<ActorPeliculasCreacionDTO> Actores { get; set; }
    }
}
