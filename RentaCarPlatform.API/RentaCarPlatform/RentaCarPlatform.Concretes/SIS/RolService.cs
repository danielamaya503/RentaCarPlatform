using Microsoft.EntityFrameworkCore;
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
    public class RolService : IRolService
    {
        private readonly RentaCarPlatformContext context;

        public RolService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<RolResponse>> Actualizar(ActualizarRolRequest request)
        {
            try
            {
                var existe = await context.Roles.FindAsync(request.RolId);

                if (existe is null)
                    return BaseResponse<RolResponse>.NotFound("Rol no encontrado.");

                var duplicado = await context.Roles
                   .AnyAsync(r => r.Nombre == request.Nombre.Trim() && r.RolId != request.RolId);

                if (duplicado)
                    return BaseResponse<RolResponse>.Conflict("Ya existe un rol con ese nombre.");

                existe.Nombre = request.Nombre.Trim();
                existe.Descripcion = request.Descripcion?.Trim();
                existe.Activo = request.Activo;

                await context.SaveChangesAsync();

                var rolResponse = new RolResponse
                {
                    RolId = existe.RolId,
                    Nombre = existe.Nombre,
                    Descripcion = existe.Descripcion,
                    Activo = existe.Activo
                };
                
                return BaseResponse<RolResponse>.Ok(rolResponse, "Rol actualizado correctamente.");


            }
            catch (DbUpdateException dbEx)
            {
                return BaseResponse<RolResponse>.Fail($"Error de base de datos al actualizar el rol: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                 return BaseResponse<RolResponse>.Fail($"Error al actualizar el rol: {ex.Message}");
            }
        }

        public async Task<BaseResponse<RolResponse>> Crear(CrearRolRequest request)
        {
            try
            {
                var existeRol = await context.Roles.AnyAsync(r => r.Nombre == request.Nombre);

                if (existeRol) 
                    return BaseResponse<RolResponse>.Conflict("Ya existe un rol con ese nombre");

                var rol = new Role
                {
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Activo = true
                };

                context.Roles.Add(rol);

                await context.SaveChangesAsync();

                var rolResponse = new RolResponse
                {
                    RolId = rol.RolId,
                    Nombre = rol.Nombre,
                    Descripcion = rol.Descripcion,
                    Activo = rol.Activo
                };

                return BaseResponse<RolResponse>.Created(rolResponse, "Rol creado correctamente.");

            }
            catch (DbUpdateException dbEx)
            {
                return BaseResponse<RolResponse>.Fail($"Error de base de datos al crear el rol: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<RolResponse>.Fail($"Error al crear el rol: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> Desactivar(int rolId)
        {
            try
            {
                var rol = await context.Roles.FirstOrDefaultAsync(r => r.RolId == rolId);

                if (rol is null)
                    return BaseResponse<bool>.NotFound("Rol no encontrado");

                var usuariosConRol = await context.Usuarios.AnyAsync(u => u.RolId == rolId);

                if (usuariosConRol)
                    return BaseResponse<bool>.Conflict("No se puede desactivar el rol porque hay usuarios asignados a él");

                rol.Activo = false;

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Rol desactivado correctamente");

            }
            catch (DbUpdateException dbEx)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar el rol: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Error al desactivar el rol: {ex.Message}");
            }
        }

        public async Task<BaseResponse<RolResponse>> ObtenerPorId(int rolId)
        {
            try
            {
                var rol = await context.Roles
                    .Where(x => x.RolId == rolId)
                    .Select(r => new RolResponse
                    {
                        RolId = r.RolId,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Activo = r.Activo
                    })
                    .FirstOrDefaultAsync();

                if (rol is null)
                    return BaseResponse<RolResponse>.NotFound("Rol no encontrado");

                return BaseResponse<RolResponse>.Ok(rol);

            }
            catch (Exception ex) 
            {
                 return BaseResponse<RolResponse>.Fail($"Error al obtener el rol: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<RolResponse>>> ObtenerTodos()
        {
            try
            {
                var roles = await context.Roles
                    .Where(r => r.Activo)
                    .Select(r => new RolResponse
                    {
                        RolId = r.RolId,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Activo = r.Activo
                    })
                    .ToListAsync();

                return BaseResponse<List<RolResponse>>.Ok(roles);
            }
            catch (Exception ex) 
            {
                return BaseResponse<List<RolResponse>>.Fail($"Error al obtener los roles: {ex.Message}");
            }
        }
    }
}
