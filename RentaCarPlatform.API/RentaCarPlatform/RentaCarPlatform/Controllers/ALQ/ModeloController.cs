using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/ModeloVehiculo")]
    [ApiController]
    [Authorize]
    public class ModeloController : ControllerBase
    {
        private readonly IModeloService modelo;

        public ModeloController(IModeloService modelo)
        {
            this.modelo = modelo;
        }

        [HttpGet("ObtenerTodos")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await modelo.ObtenerTodosAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ObtenerPorMarca/{marcaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorMarca(int marcaId)
        {
            var result = await modelo.ObtenerPorMarcaIdAsync(marcaId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ObtenerPorId/{modeloId}")]
        public async Task<IActionResult> ObtenerPorId(int modeloId)
        {
            var result = await modelo.ObtenerPorIdAsync(modeloId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearModeloRequest request)
        {
            var result = await modelo.CrearAsync(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarModeloRequest request)
        {
            var result = await modelo.ActualizarAsync(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Desactivar/{modeloId}")]
        public async Task<IActionResult> Desactivar(int modeloId)
        {
            var result = await modelo.DesactivarAsync(modeloId);

            return StatusCode(result.StatusCode, result);

        }
    }
}
