using _02_ApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _02_ApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
    }
}
