using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Clientes;

public class ActualizarClienteDto
{
    [MaxLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres")]
    public string? DireccionReferencia { get; set; }
}