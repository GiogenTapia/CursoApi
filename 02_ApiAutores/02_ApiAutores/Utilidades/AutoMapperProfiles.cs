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

            CreateMap<LibroCreacionDTO, Libro>();
            CreateMap<Libro, LibroDTO>();

            CreateMap<ComentarioCreacionDTO, Comentario>(); 
            CreateMap<Comentario, ComentarioDTO>();
        }
    }
}
