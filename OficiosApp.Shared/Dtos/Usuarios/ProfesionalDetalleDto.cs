namespace OficiosApp.Shared.Dtos.Usuarios;

public class ProfesionalDetalleDto : UsuarioDto
{
    public string? Biografia { get; set; }
    public decimal? TarifaBase { get; set; }
    public bool DisponibleUrgencia { get; set; }
    public double CalificacionPromedio { get; set; }
    public List<string> Categorias { get; set; } = new();
    public List<string> Zonas { get; set; } = new();
}