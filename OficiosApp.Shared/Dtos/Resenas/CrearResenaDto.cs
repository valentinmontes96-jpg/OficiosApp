using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Resenas;

public class CrearResenaDto
{
    [Required]
    public int IdCita { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
    public byte Puntuacion { get; set; }

    [MaxLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres")]
    public string? Comentario { get; set; }
}