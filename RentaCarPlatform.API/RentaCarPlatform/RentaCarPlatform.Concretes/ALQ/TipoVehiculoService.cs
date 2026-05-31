using Microsoft.EntityFrameworkCore;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.Models.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace RentaCarPlatform.Concretes.ALQ
{
    public class TipoVehiculoService : ITipoVehiculoService
    {
        private readonly RentaCarPlatformContext context;

        public TipoVehiculoService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<TipoVehiculoResponse>> ActualizarAsync(ActualizarTipoVehiculoRequest request)
        {
            try
            {
                var existe = await context.TiposVehiculos.FindAsync(request.TipoVehiculoId);

                if(existe is null)
                    return BaseResponse<TipoVehiculoResponse>.NotFound("Tipo de vehículo no encontrado.");

                var duplicado = await context.TiposVehiculos
                    .AnyAsync(tv => tv.Nombre.ToLower() == request.Nombre.ToLower() && tv.TipoVehiculoId != request.TipoVehiculoId);

                if (duplicado)
                    return BaseResponse<TipoVehiculoResponse>.Conflict("Ya existe otro tipo de vehículo con el mismo nombre.");

                existe.Nombre = request.Nombre;
                existe.Activo = request.Activo;

                await context.SaveChangesAsync();

                var response = new TipoVehiculoResponse
                {
                    TipoVehiculoId = existe.TipoVehiculoId,
                    Nombre = existe.Nombre,
                    Activo = existe.Activo
                };

                return BaseResponse<TipoVehiculoResponse>.Ok(response, "Tipo de vehículo actualizado exitosamente.");
            }
            catch (DbException ex)
            {
                return BaseResponse<TipoVehiculoResponse>.Fail($"Error de base de datos al actualizar el tipo de vehículo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<TipoVehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<TipoVehiculoResponse>> CrearAsync(CrearTipoVehiculoRequest request)
        {
            try
            {
                var existe = await context.TiposVehiculos.AnyAsync(tv => tv.Nombre.ToLower() == request.Nombre.ToLower());

                if (existe) 
                    return BaseResponse<TipoVehiculoResponse>.Conflict("Ya existe un tipo de vehículo con el mismo nombre.");

                var nuevoTipo = new TiposVehiculo
                {
                    Nombre = request.Nombre,
                    Activo = request.Activo
                };

                context.TiposVehiculos.Add(nuevoTipo);

                await context.SaveChangesAsync();

                var response = new TipoVehiculoResponse
                {
                    TipoVehiculoId = nuevoTipo.TipoVehiculoId,
                    Nombre = nuevoTipo.Nombre,
                    Activo = nuevoTipo.Activo
                };

                return BaseResponse<TipoVehiculoResponse>.Created(response, "Tipo de vehículo creado exitosamente.");

            }
            catch (DbException ex)
            {
                return BaseResponse<TipoVehiculoResponse>.Fail($"Error de base de datos al crear el tipo de vehículo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<TipoVehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DesactivarAsync(int tipoVehiculoId)
        {
            try
            {
                var tipo = await context.TiposVehiculos.FindAsync(tipoVehiculoId);

                if (tipo is null)
                    return BaseResponse<bool>.NotFound("Tipo de vehículo no encontrado.");

                var tieneVehiculos = await context.Vehiculos
                    .AnyAsync(v => v.TipoVehiculoId == tipoVehiculoId);

                if (tieneVehiculos)
                    return BaseResponse<bool>.Conflict("No se puede desactivar el tipo de vehículo porque tiene vehículos asociados.");

                tipo.Activo = false;
                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true);
            }
            catch (DbException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar el tipo de vehículo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<TipoVehiculoResponse>> ObtenerPorIdAsync(int tipoVehiculoId)
        {
            try
            {
                var existeTipo = await context.TiposVehiculos.AnyAsync(tv => tv.TipoVehiculoId == tipoVehiculoId && tv.Activo);

                if(!existeTipo)
                    return BaseResponse<TipoVehiculoResponse>.NotFound("No se encontró un tipo de vehículo activo con el ID proporcionado.");

                var tipoVehiculo = await context.TiposVehiculos
                    .Where(tv => tv.TipoVehiculoId == tipoVehiculoId && tv.Activo)
                    .Select(tv => new TipoVehiculoResponse
                    {
                        TipoVehiculoId = tv.TipoVehiculoId,
                        Nombre = tv.Nombre,
                        Activo = tv.Activo
                    })
                    .FirstOrDefaultAsync();

                return BaseResponse<TipoVehiculoResponse>.Ok(tipoVehiculo!);
            }
            catch (Exception ex)
            {
                return BaseResponse<TipoVehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<TipoVehiculoResponse>>> ObtenerTodosAsync()
        {
            try
            {
                var tiposVehiculos = await context.TiposVehiculos
                    .Where(tv => tv.Activo)
                    .Select(tv => new TipoVehiculoResponse
                    {
                        TipoVehiculoId = tv.TipoVehiculoId,
                        Nombre = tv.Nombre,
                        Activo = tv.Activo
                    })
                    .ToListAsync();
                
                return BaseResponse<List<TipoVehiculoResponse>>.Ok(tiposVehiculos);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<TipoVehiculoResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
