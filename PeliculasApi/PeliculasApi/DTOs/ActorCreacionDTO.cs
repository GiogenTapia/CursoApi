﻿using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [PesoArchivoValidacion(PesoMaximoEnMegaBytes:4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
