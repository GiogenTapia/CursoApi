using _02_ApiAutores.Entidades;
using Microsoft.EntityFrameworkCore;


namespace _02_ApiAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Nombre de la tabla a crear
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
    }
}
