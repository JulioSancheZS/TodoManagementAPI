using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoManagementAPI.DTO;
using TodoManagementAPI.Models;
using TodoManagementAPI.Repository.Contrato;
using TodoManagementAPI.Repository.Implementacion;
using TodoManagementAPI.Utilidades;

namespace TodoManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class TareaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITareaRepositoty _tareaRepositoty;
        public TareaController(ITareaRepositoty tareaRepositoty, IMapper mapper)
        {
            _mapper = mapper;
            _tareaRepositoty = tareaRepositoty;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(int? idEstadoTarea)
        {
            ResponseApi<List<TareaDTO>> _response = new ResponseApi<List<TareaDTO>>();

            try
            {
                List<TareaDTO> lista = new List<TareaDTO>();

                int? idUsuario = ObtenerIdUsuarioDesdeToken();

                // Utilizando expresión condicional para determinar el filtro
                Expression<Func<Tarea, bool>> filtro = x => x.IdUsuario == idUsuario && (!idEstadoTarea.HasValue || x.IdEstadoTarea == idEstadoTarea);

                IQueryable<Tarea> query = await _tareaRepositoty.Consultar(filtro);
                lista = _mapper.Map<List<TareaDTO>>(query.ToList());

                if (lista.Count > 0)
                    _response = new ResponseApi<List<TareaDTO>>() { status = true, msg = "ok", value = lista };
                else
                    _response = new ResponseApi<List<TareaDTO>>() { status = false, msg = "No hay tareas", value = null };

                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                _response = new ResponseApi<List<TareaDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        private int? ObtenerIdUsuarioDesdeToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario");

            if (claim != null && int.TryParse(claim.Value, out int idUsuario))
            {
                return idUsuario;
            }

            // Manejar el caso donde no se puede obtener el IdUsuario
            return null;
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> Guardar([FromBody] TareaDTO request)
        {
            ResponseApi<TareaDTO> _response = new ResponseApi<TareaDTO>();
            try
            {
                Tarea _tarea = _mapper.Map<Tarea>(request);

                Tarea _tareaCreada = await _tareaRepositoty.Crear(_tarea);

                if (_tareaCreada.IdTarea != 0)
                    _response = new ResponseApi<TareaDTO>() { status = true, msg = "ok", value = _mapper.Map<TareaDTO>(_tareaCreada) };
                else
                    _response = new ResponseApi<TareaDTO>() { status = false, msg = "No se pudo crear la tarea" };

                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                _response = new ResponseApi<TareaDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] TareaDTO request)
        {
            ResponseApi<TareaDTO> _response = new ResponseApi<TareaDTO>();
            try
            {
                Tarea _tarea = _mapper.Map<Tarea>(request);
                Tarea _tareaParaEditar = await _tareaRepositoty.Obtener(u => u.IdTarea == _tarea.IdTarea);

                if (_tareaParaEditar != null)
                {

                    _tareaParaEditar.Titulo = _tarea.Titulo;
                    _tareaParaEditar.Descripcion = _tarea.Descripcion;
                    _tareaParaEditar.IdEstadoTarea = _tarea.IdEstadoTarea;
                  
                    bool respuesta = await _tareaRepositoty.Editar(_tareaParaEditar);

                    if (respuesta)
                        _response = new ResponseApi<TareaDTO>() { status = true, msg = "ok", value = _mapper.Map<TareaDTO>(_tareaParaEditar) };
                    else
                        _response = new ResponseApi<TareaDTO>() { status = false, msg = "No se pudo editar la tarea" };
                }
                else
                {
                    _response = new ResponseApi<TareaDTO>() { status = false, msg = "No se encontró la tarea" };
                }

                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                _response = new ResponseApi<TareaDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
