using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Concretes.ALQ;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/vehiculo-imagen")]
    [ApiController]
    [Authorize]
    public class VehiculoImagenController : ControllerBase
    {
        private readonly IVehiculoImagenService _imagenService;

        public VehiculoImagenController(IVehiculoImagenService imagenService)
        {
            _imagenService = imagenService;
        }

        [AllowAnonymous]
        [HttpGet("{vehiculoId:int}")]
        public async Task<IActionResult> ObtenerPorVehiculo(int vehiculoId)
        {
            var response = await _imagenService.ObtenerPorVehiculoAsync(vehiculoId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearVehiculoImagenesRequest request)
        {
            var response = await _imagenService.CrearAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarVehiculoImagenRequest request)
        {
            var response = await _imagenService.ActualizarAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("principal/{imagenId:int}")]
        public async Task<IActionResult> EstablecerComoPrincipal(int imagenId)
        {
            var response = await _imagenService.EstablecerComoPrincipalAsync(imagenId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("orden")]
        public async Task<IActionResult> Reordenar([FromBody] ReordenarVehiculoImagenesRequest request)
        {
            var response = await _imagenService.ReordenarAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{imagenId:int}")]
        public async Task<IActionResult> Eliminar(int imagenId)
        {
            var response = await _imagenService.EliminarAsync(imagenId);
            return StatusCode(response.StatusCode, response);
        }


    }
}
