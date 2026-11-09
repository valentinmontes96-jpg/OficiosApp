using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OficiosApp.Shared.Dtos.Citas
{
    public class ActualizarCitaDto
    {
        [Required]
        public DateTime FechaServicioPactada { get; set; }

        [Required]
        public string DescripcionProblema { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string DireccionServicio { get; set; } = string.Empty;

        public int? IdEstadoCita { get; set; }
        public DateTime? FechaRespuesta { get; set; }
    }
}
