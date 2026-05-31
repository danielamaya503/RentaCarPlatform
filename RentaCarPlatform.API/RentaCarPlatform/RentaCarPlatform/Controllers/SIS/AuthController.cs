using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RentaCarPlatform.Concretes.SIS;
using RentaCarPlatform.Helpers;
using RentaCarPlatform.Interfaces.SIS;
using RentaCarPlatform.ViewModels.SIS.Request;
using RentaCarPlatform.ViewModels.Utilidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentaCarPlatform.Controllers.SIS
{
    [Route("api/Authentificate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService auth;
        private readonly IConfiguration configuration;

        public AuthController(IAuthService auth, IConfiguration configuration)
        {
            this.auth = auth;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await auth.LoginAsync(request);

            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("verificar")]
        public IActionResult Verificar()
        {
            var usuarioId = User.GetUsuarioId();
            var nombreUsuario = User.GetNombreUsuario();
            var email = User.GetEmail();
            var rol = User.GetRol();

            var result = BaseResponse<object>.Ok(new
            {
                usuarioId,
                nombreUsuario,
                email,
                rol
            }, "Token válido.");

            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var nombreUsuario = User.GetNombreUsuario();

            var result = BaseResponse<object>.Ok(
                new { nombreUsuario },
                "Sesión cerrada correctamente."
            );

            return StatusCode(result.StatusCode, result);
        }

        /*[AllowAnonymous]
        [HttpPost("pass:{contrasena}")]
        public string Pass( string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }*/
    }
}
