using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/estado-vehiculos")]
    [ApiController]
    [Authorize]
    public class EstadoVehiculoController : ControllerBase
    {
        private readonly IEstadoVehiculoService service;

        public EstadoVehiculoController(IEstadoVehiculoService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await service.ObtenerTodosAsync();
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpGet("{estadoVehiculoId}")]
        public async Task<IActionResult> Obtener(int estadoVehiculoId)
        {
            var result = await service.ObtenerPorIdAsync(estadoVehiculoId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEstadoVehiculoRequest request)
        {
            var result = await service.CrearAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarEstadoVehiculoRequest request)
        {
            var result = await service.ActualizarAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{estadoVehiculoId}")]
        public async Task<IActionResult> Desactivar(int estadoVehiculoId)
        {
            var result = await service.DesactivarAsync(estadoVehiculoId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
