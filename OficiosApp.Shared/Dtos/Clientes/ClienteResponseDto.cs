namespace OficiosApp.Shared.Dtos.Clientes;

public class ClienteResponseDto
{
    public int IdUsuario { get; set; }

    // Datos heredados de Usuario para no hacer doble consulta desde el frontend
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    public string? DireccionReferencia { get; set; }
}