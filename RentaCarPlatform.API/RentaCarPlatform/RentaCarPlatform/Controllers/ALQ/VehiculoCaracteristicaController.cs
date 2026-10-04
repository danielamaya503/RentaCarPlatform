using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Concretes.ALQ;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;

namespace RentaCarPlatform.Controllers.ALQ
{
    [Route("api/vehiculos-caracteristica")]
    [ApiController]
    [Authorize]
    public class VehiculoCaracteristicaController : ControllerBase
    {
        private readonly IVehiculoCaracteristicaService caracteristica;

        public VehiculoCaracteristicaController(IVehiculoCaracteristicaService caracteristica)
        {
            this.caracteristica = caracteristica;
        }

        [AllowAnonymous]
        [HttpGet("{vehiculoId:int}")]
        public async Task<IActionResult> ObtenerPorVehiculo(int vehiculoId)
        {
            var response = await caracteristica.ObtenerPorVehiculoAsync(vehiculoId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Asignar([FromBody] AsignarCaracteristicasRequest request)
        {
            var response = await caracteristica.AsignarAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("reemplazar")]
        public async Task<IActionResult> ReemplazarTodas([FromBody] ReemplazarCaracteristicasRequest request)
        {
            var response = await caracteristica.ReemplazarTodasAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> Remover([FromBody] RemoverCaracteristicasRequest request)
        {
            var response = await caracteristica.RemoverAsync(request);
            return StatusCode(response.StatusCode, response);
        }


    }
}
