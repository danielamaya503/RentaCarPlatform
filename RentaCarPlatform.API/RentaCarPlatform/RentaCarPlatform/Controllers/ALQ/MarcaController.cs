using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/MarcaVehiculo")]
    [ApiController]
    [Authorize]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaService marca;

        public MarcaController(IMarcaService marca)
        {
            this.marca = marca;
        }

        [AllowAnonymous]
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await marca.ObtenerTodosAsync();

            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpGet("ObtenerId/{id:int}")]
        public async Task<IActionResult> ObtenerId(int id)
        {
            var result = await marca.ObtenerPorIdAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearMarcaRequest request)
        {
            var result = await marca.CrearAsync(request);

            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarMarcaRequest request)
        {
            var result = await marca.ActualizarAsync(request);

            return StatusCode(result.StatusCode, result);

        }

        [HttpDelete("Desactivar/{id:int}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var result = await marca.DesactivarAsync(id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
