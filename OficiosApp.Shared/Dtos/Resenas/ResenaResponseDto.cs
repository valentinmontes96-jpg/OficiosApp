namespace OficiosApp.Shared.Dtos.Resenas;

public class ResenaResponseDto
{
    public int IdResena { get; set; }
    public int IdCita { get; set; }
    public byte Puntuacion { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaCreacion { get; set; }
}