namespace OficiosApp.Shared.Dtos.Profesionales;

public class ProfesionalResponseDto
{
    public int IdUsuario { get; set; }

    // Traemos datos del usuario para que el frontend no tenga que hacer 2 peticiones
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;

    public string? Biografia { get; set; }
    public decimal? TarifaBase { get; set; }
    public bool DisponibleUrgencia { get; set; }
    public DateTime? UltimaActualizacionDisponibilidad { get; set; }

    // Listas simples con los IDs de sus categorías y zonas asignadas
    public List<int> IdsCategorias { get; set; } = new();
    public List<int> IdsZonas { get; set; } = new();
}