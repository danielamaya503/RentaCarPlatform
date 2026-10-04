using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/caracteristicas-vehiculos")]
    [ApiController]
    [Authorize]
    public class CaracteristicaController : ControllerBase
    {
        private readonly ICaracteristicaService service;

        public CaracteristicaController(ICaracteristicaService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet("caracteristicas")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await service.ObtenerTodosAsync();
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpGet("caracteristica/{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var result = await service.ObtenerPorIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("caracteristica")]
        public async Task<IActionResult> Crear([FromBody] CrearCaracteristicaRequest request)
        {
            var result = await service.CrearAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("caracteristica")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCaracteristicaRequest request)
        { 
            var result = await service.ActualizarAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("caracteristicas/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await service.DesactivarAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
