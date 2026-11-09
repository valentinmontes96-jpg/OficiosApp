using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Citas.Request;
using OficiosApp.Shared.Dtos.Citas.Response;
using OficiosApp.Shared.Dtos.Citas;
using OficiosApp.Shared.Wrappers;

namespace OficiosApp.Application.Services;

public class CitaService : ICitaService
{
    private readonly IUnitOfWork _unitOfWork;

    public CitaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<CitaResponseDto>> SolicitarCitaAsync(CitaRequestDto request, int idUsuarioCliente)
    {
        // Actualizado a ObtenerPorIdAsync
        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(request.IdUsuarioProfesional);
        if (profesional == null)
            return new ApiResponse<CitaResponseDto>("El profesional seleccionado no existe o no está disponible.");

        // Actualizado a EstadosCita.BuscarAsync
        var estados = await _unitOfWork.EstadosCita.BuscarAsync(e => e.Nombre == "Pendiente");
        var estadoPendiente = estados.FirstOrDefault();

        if (estadoPendiente == null)
            return new ApiResponse<CitaResponseDto>("Error interno de configuración de estados.");

        var nuevaCita = new Cita
        {
            IdUsuarioCliente = idUsuarioCliente,
            IdUsuarioProfesional = request.IdUsuarioProfesional,
            IdEstadoCita = estadoPendiente.IdEstadoCita,
            FechaSolicitud = DateTime.UtcNow,
            FechaServicioPactada = request.FechaServicioPactada,
            DescripcionProblema = request.DescripcionProblema,
            DireccionServicio = request.DireccionServicio
        };

        // Actualizado a AgregarAsync
        await _unitOfWork.Citas.AgregarAsync(nuevaCita);

        var historial = new HistorialEstadoCita
        {
            Cita = nuevaCita,
            IdEstadoCita = estadoPendiente.IdEstadoCita,
            IdUsuarioAccion = idUsuarioCliente,
            Observaciones = "Cita solicitada por el cliente."
        };

        // Actualizado a GuardarCambiosAsync
        await _unitOfWork.GuardarCambiosAsync();

        var response = new CitaResponseDto
        {
            IdCita = nuevaCita.IdCita,
            Estado = "Pendiente",
            FechaSolicitud = nuevaCita.FechaSolicitud,
            FechaServicioPactada = nuevaCita.FechaServicioPactada,
            DescripcionProblema = nuevaCita.DescripcionProblema,
            DireccionServicio = nuevaCita.DireccionServicio,
            TelefonoContacto = null
        };

        return new ApiResponse<CitaResponseDto>(response, "Cita solicitada correctamente. Esperando confirmación del profesional.");
    }

    public async Task<ApiResponse<bool>> AceptarCitaAsync(int idCita, int idUsuarioProfesional)
    {
        // Actualizado a ObtenerPorIdAsync
        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(idCita);

        if (cita == null)
            return new ApiResponse<bool>("La cita no existe.");

        if (cita.IdUsuarioProfesional != idUsuarioProfesional)
            return new ApiResponse<bool>("No tienes permiso para modificar esta cita.");

        // Actualizado a EstadosCita.BuscarAsync
        var estados = await _unitOfWork.EstadosCita.BuscarAsync(e => e.Nombre == "Aceptada");
        var estadoAceptada = estados.FirstOrDefault();

        if (cita.IdEstadoCita == estadoAceptada?.IdEstadoCita)
            return new ApiResponse<bool>("La cita ya se encontraba aceptada.");

        cita.IdEstadoCita = estadoAceptada!.IdEstadoCita;
        cita.FechaRespuesta = DateTime.UtcNow;

        // Actualizado a Actualizar y GuardarCambiosAsync
        _unitOfWork.Citas.Actualizar(cita);
        await _unitOfWork.GuardarCambiosAsync();

        return new ApiResponse<bool>(true, "Cita aceptada. El cliente ahora puede ver tu número de contacto.");
    }
}