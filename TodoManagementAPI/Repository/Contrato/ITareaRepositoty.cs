using System.Linq.Expressions;
using TodoManagementAPI.Models;

namespace TodoManagementAPI.Repository.Contrato
{
    public interface ITareaRepositoty
    {
        Task<Tarea> Obtener(Expression<Func<Tarea, bool>> filtro);
        Task<Tarea> Crear(Tarea tarea);
        Task<bool> Editar(Tarea entidad);
        Task<bool> Eliminar(Tarea entidad);
        Task<IQueryable<Tarea>> Consultar(Expression<Func<Tarea, bool>> filtro = null);
    }
}
