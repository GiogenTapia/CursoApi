namespace PeliculasApi.DTOs
{
    public class FiltroPeliculasDTO
    {
        public int Pagina { get; set; } = 1;
        public int CantidadResgistrosPorPagina { get; set; } = 10;

        public PaginacionDTO Paginacion { 
            get { return new PaginacionDTO() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadResgistrosPorPagina }; }
        }

        public string Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximosEstrenos { get; set; }
        public string CampoOrdenar { get; set; }
        public bool OrdenarAscendente { get; set; }

    }
}
