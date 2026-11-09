using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Notificaciones;

public class CrearNotificacionDto
{
    [Required]
    public int IdUsuarioDestinatario { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El mensaje es obligatorio.")]
    [MaxLength(255, ErrorMessage = "El mensaje no puede superar los 255 caracteres.")]
    public string Mensaje { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    public int? ReferenciaId { get; set; }
}