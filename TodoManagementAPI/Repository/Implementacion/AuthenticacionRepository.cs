using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoManagementAPI.Models;
using TodoManagementAPI.Repository.Contrato;

namespace TodoManagementAPI.Repository.Implementacion
{
    public class AuthenticacionRepository : IAuthenticationRepository
    {
        private readonly TodoManagementApiContext _context;

        public AuthenticacionRepository(TodoManagementApiContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Obtener(Expression<Func<Usuario, bool>> filtro)
        {
            try
            {
                return await _context.Usuarios.Where(filtro).FirstOrDefaultAsync();
            }
            catch 
            {

                throw;
            }
        }
    }
}
