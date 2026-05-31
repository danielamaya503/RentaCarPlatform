using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentaCarPlatform.Interfaces.SIS;
using RentaCarPlatform.Models.SIS;
using RentaCarPlatform.ViewModels.SIS.Request;
using RentaCarPlatform.ViewModels.SIS.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Concretes.SIS
{
    public class UsuarioService : IUsuarioService
    {
        private readonly RentaCarPlatformContext context;

        public UsuarioService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<UsuarioResponse>> ActualizarAsync(ActualizarUsuarioRequest request)
        {
            try
            {
                var usuario = await context.Usuarios.FindAsync(request.UsuarioId);

                if(usuario is null)
                    return BaseResponse<UsuarioResponse>.NotFound("Usuario no encontrado.");

                var duplicado = await context.Usuarios.AnyAsync(u =>
                        u.UsuarioId != request.UsuarioId &&
                        (u.NombreUsuario == request.NombreUsuario || u.Email == request.Email));

                if (duplicado)
                    return BaseResponse<UsuarioResponse>.Conflict("Ya existe otro usuario con ese nombre o correo.");

                usuario.RolId = request.RolId;
                usuario.Email = request.Email.Trim();
                usuario.NombreUsuario = request.NombreUsuario.Trim();
                usuario.Activo = request.Activo;

                await context.SaveChangesAsync();

                var response = await context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.UsuarioId == usuario.UsuarioId)
                    .Select(u => new UsuarioResponse
                    {
                        UsuarioId = u.UsuarioId,
                        RolId = u.RolId,
                        RolNombre = u.Rol.Nombre,
                        NombreUsuario = u.NombreUsuario,
                        Email = u.Email,
                        Activo = u.Activo,
                        UltimoAcceso = u.UltimoAcceso,
                        CreadoPorUsuarioId = u.CreadoPorUsuarioId,
                        FechaCreacion = u.FechaCreacion
                    })
                    .FirstAsync();

                return BaseResponse<UsuarioResponse>.Ok(response, "Usuario actualizado correctamente.");
            }
            catch (Exception ex) 
            {
                return BaseResponse<UsuarioResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }   

        public async Task<BaseResponse<bool>> CambiarContrasenaAsync(CambiarContrasenaRequest request)
        {
            try 
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == request.UsuarioId);
                
                if(usuario is null)
                    return BaseResponse<bool>.NotFound("Usuario no encontrado.");

                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(request.NuevaContrasena);

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Contraseña actualizada correctamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al cambiar la contraseña: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<UsuarioResponse>> CrearAsync(CrearUsuarioRequest request)
        {
            try
            {
                var existeUsuario = await context.Usuarios.AnyAsync(x => x.NombreUsuario == request.NombreUsuario || x.Email == request.Email);

                if(existeUsuario)
                    return BaseResponse<UsuarioResponse>.Conflict("Ya existe un usuario con ese nombre o correo.");

                var rolExiste = await context.Roles.AnyAsync(x => x.RolId == request.RolId);

                if (!rolExiste)
                    return BaseResponse<UsuarioResponse>.NotFound("El rol especificado no existe.");

                var usuario = new Usuario
                {
                    RolId = request.RolId,
                    NombreUsuario = request.NombreUsuario.Trim(),
                    Email = request.Email.Trim(),
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(request.Contrasena),
                    Activo = request.Activo,
                    FechaCreacion = DateTime.Now,
                    CreadoPorUsuarioId = request.CreadoPorUsuarioId
                };

                context.Usuarios.Add(usuario);

                await context.SaveChangesAsync();

                var response = await context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.UsuarioId == usuario.UsuarioId)
                .Select(u => new UsuarioResponse
                {
                    UsuarioId = u.UsuarioId,
                    RolId = u.RolId,
                    RolNombre = u.Rol.Nombre,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Activo = u.Activo,
                    UltimoAcceso = u.UltimoAcceso,
                    CreadoPorUsuarioId = u.CreadoPorUsuarioId,
                    FechaCreacion = u.FechaCreacion
                })
                .FirstAsync();

                return BaseResponse<UsuarioResponse>.Created(response, "Usuario creado correctamente.");

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<UsuarioResponse>.Fail($"Error de base de datos al crear el usuario: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<UsuarioResponse>.Fail($"Error al crear el usuario: {ex.Message}");
            }
        }



        public async Task<BaseResponse<bool>> DesactivarAsync(int usuarioId)
        {
            try
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

                if (usuario is null)
                    return BaseResponse<bool>.NotFound("Usuario no encontrado.");

                usuario.Activo = false;

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Usuario desactivado correctamente.");


            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar el usuario: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Error al desactivar el usuario: {ex.Message}");
            }
        }

        public async Task<BaseResponse<UsuarioResponse>> ObtenerPorIdAsync(int usuarioId)
        {
            try
            {
                var usuario = await context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.UsuarioId == usuarioId)
                .Select(u => new UsuarioResponse
                {
                    UsuarioId = u.UsuarioId,
                    RolId = u.RolId,
                    RolNombre = u.Rol.Nombre,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Activo = u.Activo,
                    UltimoAcceso = u.UltimoAcceso,
                    CreadoPorUsuarioId = u.CreadoPorUsuarioId,
                    FechaCreacion = u.FechaCreacion
                })
                .FirstOrDefaultAsync();

                if (usuario is null)
                    return BaseResponse<UsuarioResponse>.NotFound("Usuario no encontrado.");

                return BaseResponse<UsuarioResponse>.Ok(usuario);

            }
            catch (Exception ex)
            {
                return BaseResponse<UsuarioResponse>.Fail($"Error al desactivar el usuario: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<UsuarioResponse>>> ObtenerTodosAsync()
        {
            try 
            { 
                var usuarios = await context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Activo)
                    .Select(u => new UsuarioResponse
                    {
                        UsuarioId = u.UsuarioId,
                        RolId = u.RolId,
                        RolNombre = u.Rol.Nombre,
                        NombreUsuario = u.NombreUsuario,
                        Email = u.Email,
                        Activo = u.Activo,
                        UltimoAcceso = u.UltimoAcceso,
                        CreadoPorUsuarioId = u.CreadoPorUsuarioId,
                        FechaCreacion = u.FechaCreacion
                    })
                    .ToListAsync();

                return BaseResponse<List<UsuarioResponse>>.Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<UsuarioResponse>>.Fail($"Error al obtener los usuarios: {ex.Message}");
            }
        }
    }
}
