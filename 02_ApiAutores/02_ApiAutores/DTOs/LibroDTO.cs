using _02_ApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _02_ApiAutores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
    }
}
