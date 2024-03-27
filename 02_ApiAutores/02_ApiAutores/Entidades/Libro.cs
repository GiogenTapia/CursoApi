using _02_ApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _02_ApiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }


    }
}
