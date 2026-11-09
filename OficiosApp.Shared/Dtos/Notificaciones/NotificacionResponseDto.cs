namespace OficiosApp.Shared.Dtos.Notificaciones;

public class NotificacionResponseDto
{
    public int IdNotificacion { get; set; }
    public int IdUsuarioDestinatario { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int? ReferenciaId { get; set; }
    public bool Leida { get; set; }
    public DateTime FechaCreacion { get; set; }
}