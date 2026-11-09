namespace OficiosApp.Shared.Dtos.Profesionales;

public class ActualizarProfesionalDto
{
    public string? Biografia { get; set; }
    public decimal? TarifaBase { get; set; }
    public bool DisponibleUrgencia { get; set; }
}