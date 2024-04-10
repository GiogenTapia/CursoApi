using System.ComponentModel.DataAnnotations;

namespace _02_ApiAutores.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
