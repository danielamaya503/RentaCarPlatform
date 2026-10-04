using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Interfaces.ALQ;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/tipo-vehiculos")]
    [ApiController]
    [Authorize]
    public class TipoVehiculoController : ControllerBase
    {
        private readonly ITipoVehiculoService service;

        public TipoVehiculoController(ITipoVehiculoService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodo()
        {
            var result = await service.ObtenerTodosAsync();

            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpGet("{tipoVehiculoId}")]
        public async Task<IActionResult> Obtener(int tipoVehiculoId)
        {
            var result = await service.ObtenerPorIdAsync(tipoVehiculoId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ViewModels.ALQ.Request.CrearTipoVehiculoRequest request)
        {
            var result = await service.CrearAsync(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] ViewModels.ALQ.Request.ActualizarTipoVehiculoRequest request)
        {
            var result = await service.ActualizarAsync(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{tipoVehiculoId}")]
        public async Task<IActionResult> Desactivar(int tipoVehiculoId)
        {
            var result = await service.DesactivarAsync(tipoVehiculoId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
