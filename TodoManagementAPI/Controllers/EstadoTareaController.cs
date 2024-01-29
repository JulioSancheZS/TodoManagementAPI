
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoManagementAPI.DTO;
using TodoManagementAPI.Models;
using TodoManagementAPI.Repository.Contrato;
using TodoManagementAPI.Utilidades;

namespace TodoManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoTareaController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IEstadoTareaRepository _estadoTareaRepository;

        public EstadoTareaController(IMapper mapper, IEstadoTareaRepository estadoTareaRepository)
        {
            _mapper = mapper;
            _estadoTareaRepository = estadoTareaRepository;
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> get()
        {
            ResponseApi<List<EstadoTareaDTO>> _response = new ResponseApi<List<EstadoTareaDTO>>();

            try
            {
                List<EstadoTareaDTO> _lista = new List<EstadoTareaDTO>();
                _lista = _mapper.Map<List<EstadoTareaDTO>>(await _estadoTareaRepository.Listado());

                if(_lista.Count > 0)
                {
                    _response = new ResponseApi<List<EstadoTareaDTO>>() { status = true, msg = "ok", value = _lista };

                }
                else
                    _response = new ResponseApi<List<EstadoTareaDTO>>() { status = false, msg = "sin resultados", value = null };

                return StatusCode(StatusCodes.Status200OK, _response);

            }
            catch (Exception ex)
            {
                _response = new ResponseApi<List<EstadoTareaDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

       
    }
}
