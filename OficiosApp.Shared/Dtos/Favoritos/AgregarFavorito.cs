using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Favoritos;

public class AgregarFavoritoDto
{
    [Required]
    public int IdUsuarioProfesional { get; set; }
}