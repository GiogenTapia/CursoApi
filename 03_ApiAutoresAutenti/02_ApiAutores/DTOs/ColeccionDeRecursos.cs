namespace _02_ApiAutores.DTOs
{
    public class ColeccionDeRecursos<T> : Recurso where T: Recurso
    {
        public List<T> Valores { get; set; }
    }
}
