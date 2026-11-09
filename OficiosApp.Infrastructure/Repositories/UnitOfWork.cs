using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Infrastructure.Data;

namespace OficiosApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IGenericRepository<Usuario> Usuarios { get; }
    public IGenericRepository<Cliente> Clientes { get; }
    public IGenericRepository<Profesional> Profesionales { get; }
    public IGenericRepository<Categoria> Categorias { get; }
    public IGenericRepository<Zona> Zonas { get; }
    public IGenericRepository<Cita> Citas { get; }
    public IGenericRepository<EstadoCita> EstadosCita { get; }
    public IGenericRepository<Resena> Resenas { get; }
    public IGenericRepository<Notificacion> Notificaciones { get; }
    public IGenericRepository<ReporteUsuario> ReportesUsuarios { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Usuarios = new GenericRepository<Usuario>(_context);
        Clientes = new GenericRepository<Cliente>(_context);
        Profesionales = new GenericRepository<Profesional>(_context);
        Categorias = new GenericRepository<Categoria>(_context);
        Zonas = new GenericRepository<Zona>(_context);
        Citas = new GenericRepository<Cita>(_context);
        EstadosCita = new GenericRepository<EstadoCita>(_context);
        Resenas = new GenericRepository<Resena>(_context);
        Notificaciones = new GenericRepository<Notificacion>(_context);
        ReportesUsuarios = new GenericRepository<ReporteUsuario>(_context);
    }

    public async Task<int> GuardarCambiosAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}

