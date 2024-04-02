using _02_ApiAutores.DTOs;
using _02_ApiAutores.Entidades;
using AutoMapper;

namespace _02_ApiAutores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //Aqui es primero el DTO porque es del cliente al servidor
            CreateMap<AutorCreacionDTO, Autor>();
            //Aqui porque es del servidor al cliente
            CreateMap<Autor, AutorDTO>();  

            //Mapeo especial, de listado de AutoresId de LibroCreacionDTO
            //con el listado de AutoresLIbros de la entidad Libro
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro=> libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            
            CreateMap<Libro, LibroDTO>();

            CreateMap<ComentarioCreacionDTO, Comentario>(); 
            CreateMap<Comentario, ComentarioDTO>();
        }

        //Funcion para mapear un entero entre los AutoresLibros
        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO,Libro libro) { 

            var resultado = new List<AutorLibro>();
            if(libroCreacionDTO.AutoresIds == null){ return resultado; }

            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }
            return resultado;
        
        }
    }
}
