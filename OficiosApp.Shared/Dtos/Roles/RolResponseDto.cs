namespace OficiosApp.Shared.Dtos.Roles;

public class RolResponseDto
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}