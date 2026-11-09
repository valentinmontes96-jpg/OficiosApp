using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("HORARIOS_ATENCION")]
public class HorarioAtencion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdHorario { get; set; }

    [Required]
    public int IdUsuarioProfesional { get; set; }

    [Required]
    public byte DiaSemana { get; set; } // 0 = Domingo, 1 = Lunes...

    [Required]
    public TimeSpan HoraApertura { get; set; }

    [Required]
    public TimeSpan HoraCierre { get; set; }

    // Propiedad de navegación
    [ForeignKey(nameof(IdUsuarioProfesional))]
    public virtual Profesional Profesional { get; set; } = null!;
}