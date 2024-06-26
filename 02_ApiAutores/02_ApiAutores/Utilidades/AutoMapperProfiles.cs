﻿using _02_ApiAutores.DTOs;
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
            CreateMap<Autor, AutorDTOLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));
            


            //Mapeo especial, de listado de AutoresId de LibroCreacionDTO
            //con el listado de AutoresLIbros de la entidad Libro
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro=> libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOAutores>()
                .ForMember(libroDTO=> libroDTO.Autores, opciones=> opciones.MapFrom(MapLibroDTOAutores));

            //Aqui se hace entre ambos
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

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


        //Mapeo especial para mostrar el id y el nombre del autor en nuestro LibroDTO
        //Obtendiendolo de AutorDTO
        //Estos son mapeos especiales
        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();
            if (libro.AutoresLibros == null) { return resultado; }
            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }
            return resultado;
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();
            if (autor.AutoresLibros == null) { return resultado; }
            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo

                });
            }
            return resultado;
        }


    }
}
