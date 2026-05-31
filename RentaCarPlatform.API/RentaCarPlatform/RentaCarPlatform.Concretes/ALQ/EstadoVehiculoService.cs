using Microsoft.EntityFrameworkCore;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.Models.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Concretes.ALQ
{
    public class EstadoVehiculoService : IEstadoVehiculoService
    {
        private static readonly string[] EstadosProtegidos = ["Disponible", "Rentado", "Mantenimiento", "Inactivo"];

        private readonly RentaCarPlatformContext context;

        public EstadoVehiculoService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<EstadoVehiculoResponse>> ActualizarAsync(ActualizarEstadoVehiculoRequest request)
        {
            try
            {
                var estado = await context.EstadosVehiculos.FindAsync(request.EstadoVehiculoId);

                if (estado is null)
                    return BaseResponse<EstadoVehiculoResponse>.NotFound("Estado de vehículo no encontrado.");

                if (EstadosProtegidos.Contains(estado.Nombre) && estado.Nombre != request.Nombre.Trim())
                    return BaseResponse<EstadoVehiculoResponse>.Conflict($"El estado '{estado.Nombre}' es un estado base del sistema y su nombre no puede modificarse porque los triggers de la base de datos dependen de él.");

                var nombre = request.Nombre.Trim();

                var duplicado = await context.EstadosVehiculos
                    .AnyAsync(e => e.EstadoVehiculoId != request.EstadoVehiculoId && e.Nombre == nombre);

                if (duplicado)
                    return BaseResponse<EstadoVehiculoResponse>.Conflict("Ya existe otro estado con ese nombre.");

                estado.Nombre = nombre;
                estado.ColorHex = request.ColorHex?.Trim().ToUpper();
                estado.Activo = request.Activo;

                await context.SaveChangesAsync();

                var response = new EstadoVehiculoResponse
                {
                    EstadoVehiculoId = estado.EstadoVehiculoId,
                    Nombre = estado.Nombre,
                    ColorHex = estado.ColorHex,
                    Activo = estado.Activo
                };

                return BaseResponse<EstadoVehiculoResponse>.Ok(response, "Estado de vehículo actualizado exitosamente.");
            }
            catch(DbUpdateException ex)
            {
                return BaseResponse<EstadoVehiculoResponse>.Fail($"Error de base de datos al actualizar el estado del vehiculo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<EstadoVehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
         
        public async Task<BaseResponse<EstadoVehiculoResponse>> CrearAsync(CrearEstadoVehiculoRequest request)
        {
            try
            {
                var existe = await context.EstadosVehiculos
                    .AnyAsync(e => e.Nombre.ToLower() == request.Nombre.ToLower());

                if (existe)
                    return BaseResponse<EstadoVehiculoResponse>.Conflict($"Ya existe un estado de vehículo con el nombre '{request.Nombre}'.");

                var nuevoEstado = new EstadosVehiculo
                {
                    Nombre = request.Nombre,
                    ColorHex = request.ColorHex,
                    Activo = request.Activo
                };

                context.EstadosVehiculos.Add(nuevoEstado);

                await context.SaveChangesAsync();

                var response = new EstadoVehiculoResponse
                {
                    EstadoVehiculoId = nuevoEstado.EstadoVehiculoId,
                    Nombre = nuevoEstado.Nombre,
                    ColorHex = nuevoEstado.ColorHex,
                    Activo = nuevoEstado.Activo
                };

                return BaseResponse<EstadoVehiculoResponse>.Created(response, "Estado de vehículo creado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<EstadoVehiculoResponse>.Fail($"Error de base de datos al crear el estado del vehículo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<EstadoVehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DesactivarAsync(int estadoVehiculoId)
        {
            try
            {
                var estado = await context.EstadosVehiculos.FindAsync(estadoVehiculoId);

                if (estado is null)
                    return BaseResponse<bool>.NotFound($"No se encontró un estado de vehículo con ID {estadoVehiculoId}.");

                if (EstadosProtegidos.Contains(estado.Nombre))
                    return BaseResponse<bool>.Fail($"El estado '{estado.Nombre}' es un estado base del sistema y no puede desactivarse.");

                var tieneVehiculos = await context.Vehiculos
                   .AnyAsync(v => v.EstadoVehiculoId == estadoVehiculoId);

                if (tieneVehiculos)
                    return BaseResponse<bool>.Fail("No se puede desactivar el estado porque tiene vehículos asociados.");

                estado.Activo = false;

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Estado de vehículo desactivado exitosamente.");

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar el estado del vehículo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<EstadoVehiculoResponse>> ObtenerPorIdAsync(int estadoVehiculoId)
        {
            try
            {
                var estado = await context.EstadosVehiculos
                    .Where(e => e.EstadoVehiculoId == estadoVehiculoId && e.Activo)
                    .Select(e => new EstadoVehiculoResponse
                    {
                        EstadoVehiculoId = e.EstadoVehiculoId,
                        Nombre = e.Nombre,
                        ColorHex = e.ColorHex,
                        Activo = e.Activo
                    })
                    .FirstOrDefaultAsync();

                if(estado is null)
                    return BaseResponse<EstadoVehiculoResponse>.NotFound($"No se encontró un estado de vehículo activo con ID {estadoVehiculoId}.");

                return BaseResponse<EstadoVehiculoResponse>.Ok(estado);
            }
            catch (Exception ex)
            {
                return BaseResponse<EstadoVehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<EstadoVehiculoResponse>>> ObtenerTodosAsync()
        {
            try
            {
                var estados = await context.EstadosVehiculos
                    .Where(e => e.Activo)
                    .OrderBy(e => e.Nombre)
                    .Select(e => new EstadoVehiculoResponse
                    {
                        EstadoVehiculoId = e.EstadoVehiculoId,
                        Nombre = e.Nombre,
                        ColorHex = e.ColorHex,
                        Activo = e.Activo
                    })
                    .ToListAsync();

                return BaseResponse<List<EstadoVehiculoResponse>>.Ok(estados);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<EstadoVehiculoResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

    }
}
