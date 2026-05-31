using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Interfaces.SIS;
using RentaCarPlatform.Models.SIS;
using RentaCarPlatform.ViewModels.SIS.Request;

namespace RentaCarPlatform.Controllers.SIS
{
    [Route("api/Roles")]
    [Authorize(Roles = "Administrador")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolService rolService;

        public RolesController(IRolService rolService)
        {
            this.rolService = rolService;
        }

       
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await rolService.ObtenerTodos();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ObtenerPorId/{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var result = await rolService.ObtenerPorId(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarRolRequest request)
        {
            var result = await rolService.Actualizar(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearRolRequest request)
        {
            var result = await rolService.Crear(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await rolService.Desactivar(id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
