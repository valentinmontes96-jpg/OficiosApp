using System.Linq.Expressions;

namespace OficiosApp.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> ObtenerPorIdAsync(int id);
    Task<IReadOnlyList<T>> ObtenerTodosAsync();
    Task<IReadOnlyList<T>> BuscarAsync(Expression<Func<T, bool>> predicado);
    Task<T> AgregarAsync(T entidad);
    void Actualizar(T entidad);
    void Eliminar(T entidad);
}