using _02_ApiAutores.Entidades;
using Microsoft.EntityFrameworkCore;


namespace _02_ApiAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        //Creación del identificador compuesto por dos datos
        //Identificador compuesto
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AutorLibro>().HasKey(al => new{al.AutorId, al.LibroId});
        }

        //Nombre de la tabla a crear
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }
}
