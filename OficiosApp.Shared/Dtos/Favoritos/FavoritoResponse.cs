namespace OficiosApp.Shared.Dtos.Favoritos;

public class FavoritoResponseDto
{
    public int IdUsuarioProfesional { get; set; }
    public string NombreProfesional { get; set; } = string.Empty;
    public string ApellidoProfesional { get; set; } = string.Empty;
    public decimal? TarifaBase { get; set; }
    public bool DisponibleUrgencia { get; set; }
    public DateTime FechaAgregado { get; set; }
}