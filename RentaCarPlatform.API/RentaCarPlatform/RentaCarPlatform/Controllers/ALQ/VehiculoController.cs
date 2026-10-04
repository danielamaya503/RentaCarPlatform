using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Helpers;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/vehiculos")]
    [ApiController]
    [Authorize]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            this.vehiculoService = vehiculoService;
        }

        
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos([FromQuery] FiltroVehiculoRequest filtro)
        {
            var response = await vehiculoService.ObtenerTodosAsync(filtro);
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var response = await vehiculoService.ObtenerPorIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }


        [AllowAnonymous]
        [HttpGet("catalogo")]
        public async Task<IActionResult> ObtenerCatalogo([FromQuery] FiltroVehiculoRequest filtro)
        {
            var response = await vehiculoService.ObtenerCatalogoAsync(filtro);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearVehiculoRequest request)
        {
            var creadoPor = User.GetUsuarioId();
            var response = await vehiculoService.CrearAsync(request, creadoPor);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarVehiculoRequest request)
        {

           var response = await vehiculoService.ActualizarAsync(request);

           return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar( int id)
        {
            var response = await vehiculoService.DesactivarAsync(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
