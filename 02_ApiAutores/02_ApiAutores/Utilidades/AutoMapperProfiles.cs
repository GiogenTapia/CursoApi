using _02_ApiAutores.DTOs;
using _02_ApiAutores.Entidades;
using AutoMapper;

namespace _02_ApiAutores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
        }
    }
}
