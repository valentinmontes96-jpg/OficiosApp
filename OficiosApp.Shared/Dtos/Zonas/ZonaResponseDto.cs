namespace OficiosApp.Shared.Dtos.Zonas;

public class ZonaResponseDto
{
    public int IdZona { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoPostal { get; set; }
    public bool Activa { get; set; }
}