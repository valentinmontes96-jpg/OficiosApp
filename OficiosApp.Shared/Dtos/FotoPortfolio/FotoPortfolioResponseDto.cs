namespace OficiosApp.Shared.Dtos.Portfolio;

public class FotoPortfolioResponseDto
{
    public int IdFoto { get; set; }
    public int IdUsuarioProfesional { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaCarga { get; set; }
}