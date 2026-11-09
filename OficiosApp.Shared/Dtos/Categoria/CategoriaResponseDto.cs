namespace OficiosApp.Shared.Dtos.Categoria
{
    public class CategoriaResponseDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? IconoUrl { get; set; }
        public bool Activa { get; set; }
    }
}
