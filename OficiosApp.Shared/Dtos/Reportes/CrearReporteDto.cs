using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Reportes;

public class CrearReporteDto
{
    [Required]
    public int IdUsuarioDenunciado { get; set; }

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El motivo no puede superar los 150 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string Descripcion { get; set; } = string.Empty;
}