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
        }
    }
}
