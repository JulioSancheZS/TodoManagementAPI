using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoManagementAPI.DTO;
using TodoManagementAPI.Models;
using TodoManagementAPI.Models.ViewModel;
using TodoManagementAPI.Repository.Contrato;
using TodoManagementAPI.Utilidades;

namespace TodoManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticacionController : ControllerBase
    {
        public readonly IAuthenticationRepository _authenticationRepository;
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;

        public AuthenticacionController(IAuthenticationRepository authenticationRepository, IMapper mapper, IConfiguration configuration)
        {
            _authenticationRepository = authenticationRepository;
            _mapper = mapper;
            _configuration = configuration; 

        }

        [HttpPost]
        public async Task<IActionResult> Authentication([FromBody] LoginViewModel loginVM)
        {

            ResponseApi<UsuarioDTO> _responseApi = new ResponseApi<UsuarioDTO>();

            try
            {
                Usuario _usuario = await _authenticationRepository.Obtener(x => x.Correo == loginVM.Email && x.Password == loginVM.Password);

                if (_usuario == null)
                {
                    _responseApi = new ResponseApi<UsuarioDTO> { status = false, msg = "No se encontro el Usuario, porfavor, intentar nuevamente" };
                    return StatusCode(StatusCodes.Status200OK, _responseApi);
                }

                //JWT
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("IdUsuario", _usuario.IdUsuario.ToString()),
                        new Claim("Usuario", _usuario.Usuario1.ToString()),
                        new Claim("Nombre", _usuario.PrimerNombre + " " + _usuario.PrimerApellido)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn);

                _responseApi = new ResponseApi<UsuarioDTO>() { status = true, msg = "ok", value = _mapper.Map<UsuarioDTO>(_usuario), token = new JwtSecurityTokenHandler().WriteToken(token) };

                 return StatusCode(StatusCodes.Status200OK, _responseApi);

            }
            catch (Exception ex)
            {
                _responseApi = new ResponseApi<UsuarioDTO>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _responseApi);
            }
        }

    }
}
