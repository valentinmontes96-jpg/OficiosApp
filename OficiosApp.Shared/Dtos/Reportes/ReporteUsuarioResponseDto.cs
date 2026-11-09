namespace OficiosApp.Shared.Dtos.Reportes;

public class ReporteUsuarioResponseDto
{
    public int IdReporte { get; set; }
    public int IdUsuarioDenunciante { get; set; }
    public string NombreDenunciante { get; set; } = string.Empty;

    public int IdUsuarioDenunciado { get; set; }
    public string NombreDenunciado { get; set; } = string.Empty;

    public string Motivo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaReporte { get; set; }
    public string Estado { get; set; } = string.Empty;

    public int? IdAdministradorResolutor { get; set; }
}