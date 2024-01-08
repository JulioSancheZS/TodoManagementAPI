using TodoManagementAPI.Models;

namespace TodoManagementAPI.Repository.Contrato
{
    public interface IEstadoTareaRepository
    {
        Task<List<EstadoTarea>> Listado();
    }
}
