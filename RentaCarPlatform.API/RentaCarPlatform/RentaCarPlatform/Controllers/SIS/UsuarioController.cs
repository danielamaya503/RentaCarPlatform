using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCarPlatform.Concretes.SIS;
using RentaCarPlatform.Helpers;
using RentaCarPlatform.Interfaces.SIS;
using RentaCarPlatform.ViewModels.SIS.Request;

namespace RentaCarPlatform.Controllers.SIS
{
    [Route("api/Usuario")]
    [Authorize(Roles = "Administrador")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuario;

        public UsuarioController(IUsuarioService usuario)
        {
            this.usuario = usuario;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await usuario.ObtenerTodosAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ById/{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuarioLogueadoId = User.GetUsuarioId();
            var rol = User.GetRol();

            if (rol != "Administrador" && usuarioLogueadoId != id)
                return Forbid();

            var result = await usuario.ObtenerPorIdAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("mi-perfil")]
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioId = User.GetUsuarioId();
            var result = await usuario.ObtenerPorIdAsync(usuarioId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.CreadoPorUsuarioId = User.GetUsuarioId();

            var result = await usuario.CrearAsync(request);

            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != request.UsuarioId)
                return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            var usuarioLogueadoId = User.GetUsuarioId();
            var rol = User.GetRol();

            if (rol != "Administrador" && usuarioLogueadoId != id)
                return Forbid();

            var result = await usuario.ActualizarAsync(request);

            return StatusCode(result.StatusCode, result);
        }


        [HttpPatch("{id:int}/cambiar-contrasena")]
        public async Task<IActionResult> CambiarContrasena(int id, [FromBody] CambiarContrasenaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != request.UsuarioId)
                return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            var usuarioLogueadoId = User.GetUsuarioId();
            var rol = User.GetRol();

            if (rol != "Administrador" && usuarioLogueadoId != id)
                return Forbid();

            var result = await usuario.CambiarContrasenaAsync(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Desactivar/{id:int}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var usuarioLogueadoId = User.GetUsuarioId();

            if (usuarioLogueadoId == id)
                return BadRequest("No puedes desactivar tu propia cuenta.");

            var result = await usuario.DesactivarAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        
    }
}
