using OficiosApp.Domain.Entities;

namespace OficiosApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Usuario> Usuarios { get; }
    IGenericRepository<Cliente> Clientes { get; }
    IGenericRepository<Profesional> Profesionales { get; }
    IGenericRepository<Categoria> Categorias { get; }
    IGenericRepository<Zona> Zonas { get; }
    IGenericRepository<Cita> Citas { get; }
    IGenericRepository<EstadoCita> EstadosCita { get; }
    IGenericRepository<Resena> Resenas { get; }
    IGenericRepository<Notificacion> Notificaciones { get; }
    IGenericRepository<ReporteUsuario> ReportesUsuarios { get; }

    // Cambiamos CompleteAsync por GuardarCambiosAsync
    Task<int> GuardarCambiosAsync();
}