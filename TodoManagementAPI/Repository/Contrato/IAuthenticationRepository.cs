using System.Linq.Expressions;
using TodoManagementAPI.Models;

namespace TodoManagementAPI.Repository.Contrato
{
    public interface IAuthenticationRepository
    {
        Task<Usuario> Obtener(Expression<Func<Usuario, bool>> filtro);
    }
}
