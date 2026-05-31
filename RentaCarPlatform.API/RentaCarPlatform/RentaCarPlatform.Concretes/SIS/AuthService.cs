using Microsoft.EntityFrameworkCore;
using RentaCarPlatform.Interfaces;
using RentaCarPlatform.Interfaces.SIS;
using RentaCarPlatform.ViewModels.SIS.Request;
using RentaCarPlatform.ViewModels.SIS.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Concretes.SIS
{
    public class AuthService : IAuthService
    {
        private readonly RentaCarPlatformContext context;
        private readonly ITokenService token;

        public AuthService(RentaCarPlatformContext context, ITokenService token)
        {
            this.context = context;
            this.token = token;
        }

        public async Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Contrasena))
                    return BaseResponse<LoginResponse>.BadRequest("Email y contraseña son requeridos.");

                var usuario = await context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Activo);

                if (usuario is null)
                    return BaseResponse<LoginResponse>.BadRequest("Credenciales inválidas.");

                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                    return BaseResponse<LoginResponse>.BadRequest("El usuario no tiene contraseña configurada.");

                if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.Contrasena))
                    return BaseResponse<LoginResponse>.BadRequest("Credenciales inválidas.");

                // Actualizar último acceso
                usuario.UltimoAcceso = DateTime.Now;
                await context.SaveChangesAsync();

                var tokenServe = token.GenerarToken(usuario, usuario.Rol.Nombre);

                var response = new LoginResponse
                {
                    Token = tokenServe,
                    NombreUsuario = usuario.NombreUsuario,
                    Email = usuario.Email,
                    Rol = usuario.Rol.Nombre,
                    Expiracion = DateTime.UtcNow.AddMinutes(480)
                };

                return BaseResponse<LoginResponse>.Ok(response, "Login exitoso.");

            }
            catch (Exception ex)
            {
                return BaseResponse<LoginResponse>.Fail($"Error al iniciar sesión: {ex.Message}");
            }
        }
    }
}
