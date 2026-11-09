using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Infrastructure.Data;

namespace OficiosApp.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ObtenerTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> BuscarAsync(Expression<Func<T, bool>> predicado)
    {
        return await _dbSet.Where(predicado).ToListAsync();
    }

    public async Task<T> AgregarAsync(T entidad)
    {
        await _dbSet.AddAsync(entidad);
        return entidad;
    }

    public void Actualizar(T entidad)
    {
        _dbSet.Update(entidad);
    }

    public void Eliminar(T entidad)
    {
        _dbSet.Remove(entidad);
    }
}