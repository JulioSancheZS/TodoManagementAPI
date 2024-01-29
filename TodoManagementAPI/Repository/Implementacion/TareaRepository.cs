using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoManagementAPI.Models;
using TodoManagementAPI.Repository.Contrato;

namespace TodoManagementAPI.Repository.Implementacion
{
    public class TareaRepository : ITareaRepositoty
    {
        public readonly TodoManagementApiContext _context;

        public TareaRepository(TodoManagementApiContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Tarea>> Consultar(Expression<Func<Tarea, bool>> filtro = null)
        {
            //IQueryable<Tarea> query = filtro == null ? _context.Tareas.OrderByDescending(x => x.IdTarea).Include(x => x.IdEstadoTareaNavigation) : _context.Tareas.Where(filtro);
            //return query;

            IQueryable<Tarea> query = _context.Tareas.OrderByDescending(x => x.IdTarea).Include(x => x.IdEstadoTareaNavigation);

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return query;
        }

        public async Task<Tarea> Crear(Tarea tarea)
        {
            try
            {
                //Tarea oTarea = new Tarea();

                //oTarea.Titulo = tarea.Titulo;
                //oTarea.Descripcion = tarea.Descripcion;
                //oTarea.IdEstadoTarea = tarea.IdEstadoTarea;
                //oTarea.IdUsuario = tarea.IdUsuario; 

                _context.Set<Tarea>().Add(tarea);
                await _context.SaveChangesAsync();
                return tarea;
            }
            catch 
            {

                throw;
            }
        }

        public async Task<bool> Editar(Tarea entidad)
        {
            try
            {
                _context.Update(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(Tarea entidad)
        {
            try
            {
                 _context.Remove(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {

                throw;
            }
        }

        public async Task<Tarea> Obtener(Expression<Func<Tarea, bool>> filtro)
        {
            try
            {
                return await _context.Tareas.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {

                throw;
            }
        }
    }
}
