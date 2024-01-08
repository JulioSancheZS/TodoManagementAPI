using Microsoft.EntityFrameworkCore;
using TodoManagementAPI.Models;
using TodoManagementAPI.Repository.Contrato;

namespace TodoManagementAPI.Repository.Implementacion
{
    public class EstadoTareaRepository : IEstadoTareaRepository
    {
        public readonly TodoManagementApiContext _context;

        public EstadoTareaRepository(TodoManagementApiContext context)
        {
            _context = context;
        }

        public async Task<List<EstadoTarea>> Listado()
        {
            try
            {
             return await _context.EstadoTareas.ToListAsync();

            }
            catch 
            {

                throw;
            }
        }
    }
}
