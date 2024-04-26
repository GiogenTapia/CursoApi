using AutoMapper;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;

namespace PeliculasApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();

            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x=> x.Foto, options=>options.Ignore());
            CreateMap<ActorPatchDTO,Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }
    }
}
