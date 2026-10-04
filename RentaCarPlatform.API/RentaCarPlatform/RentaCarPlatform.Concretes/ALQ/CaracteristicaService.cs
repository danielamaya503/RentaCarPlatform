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
    public class CaracteristicaService : ICaracteristicaService
    {
        private readonly RentaCarPlatformContext _context;

        public CaracteristicaService(RentaCarPlatformContext context) {
            _context = context;
        }

        public async Task<BaseResponse<CaracteristicaResponse>> ActualizarAsync(ActualizarCaracteristicaRequest request)
        {
            try
            {
                var caracteristica = await _context.Caracteristicas.FirstOrDefaultAsync(c => c.CaracteristicaId == request.CaracteristicaId);

                if(caracteristica is null)
                    return BaseResponse<CaracteristicaResponse>.NotFound($"No se encontró la característica con ID {request.CaracteristicaId}.");

                var duplicado = await _context.Caracteristicas.AnyAsync(c => c.CaracteristicaId == request.CaracteristicaId && c.Nombre == request.Nombre);

                if(duplicado)
                    return BaseResponse<CaracteristicaResponse>.Conflict("Ya existe otra característica con ese nombre.");

                caracteristica.Nombre = request.Nombre.Trim();
                caracteristica.Icono = string.IsNullOrWhiteSpace(request.Icono) ? null : request.Icono.Trim();
                caracteristica.Activo = request.Activo;

                await _context.SaveChangesAsync();

                var response = new CaracteristicaResponse
                {
                    CaracteristicaId = caracteristica.CaracteristicaId,
                    Nombre = caracteristica.Nombre,
                    Icono = caracteristica.Icono,
                    Activo = caracteristica.Activo
                };

                return BaseResponse<CaracteristicaResponse>.Ok(response, "Característica actualizada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<CaracteristicaResponse>.Fail($"Error de base de datos al actualizar la característica: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<CaracteristicaResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CaracteristicaResponse>> CrearAsync(CrearCaracteristicaRequest request)
        {
            try
            {
                var icono = string.IsNullOrWhiteSpace(request.Icono) ? null : request.Icono.Trim();
                var existe = await _context.Caracteristicas.FirstOrDefaultAsync(c => c.Nombre == request.Nombre.Trim());

                if (existe is not null)
                {
                    return BaseResponse<CaracteristicaResponse>.Conflict("Ya existe una característica con ese nombre.");
                }

                var caracteristica = new Caracteristica
                {
                    Nombre = request.Nombre.Trim(),
                    Icono = request.Icono,
                    Activo = true
                };

                _context.Caracteristicas.Add(caracteristica);

                await _context.SaveChangesAsync();

                var response = new CaracteristicaResponse
                {
                    CaracteristicaId = caracteristica.CaracteristicaId,
                    Nombre = caracteristica.Nombre,
                    Icono = caracteristica.Icono,
                    Activo = caracteristica.Activo
                };

                return BaseResponse<CaracteristicaResponse>.Created(response, "Característica creada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<CaracteristicaResponse>.Fail($"Error de base de datos al crear la característica: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<CaracteristicaResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DesactivarAsync(int caracteristicaId)
        {
            try
            {
                var existe = await _context.Caracteristicas.FirstOrDefaultAsync(c => c.CaracteristicaId == caracteristicaId && c.Activo);

                if (existe is null)
                    return BaseResponse<bool>.NotFound($"No se encontró la característica con ID {caracteristicaId}.");

                var asignado = await _context.VehiculoCaracteristicas.AnyAsync(vc => vc.CaracteristicaId == caracteristicaId);

                if (asignado)
                    return BaseResponse<bool>.Conflict($"La característica con ID {caracteristicaId} está asignada a algún vehículo y no puede ser desactivada.");
              
                existe.Activo = false;

                await _context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Característica desactivada exitosamente.");

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar la característica: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CaracteristicaResponse>> ObtenerPorIdAsync(int caracteristicaId)
        {
            try
            {
                var existe = await _context.Caracteristicas
                    .Where(c => c.CaracteristicaId == caracteristicaId && c.Activo)
                    .Select(c => new CaracteristicaResponse
                    {
                        CaracteristicaId = c.CaracteristicaId,
                        Nombre = c.Nombre,
                        Icono = c.Icono,
                        Activo = c.Activo
                    }) .FirstOrDefaultAsync();

                if (existe is null)
                    return BaseResponse<CaracteristicaResponse>.NotFound($"No se encontró la característica con ID {caracteristicaId}.");

                return BaseResponse<CaracteristicaResponse>.Ok(existe, "Característica obtenida exitosamente.");


            }
            catch (Exception ex)
            {
                return BaseResponse<CaracteristicaResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<CaracteristicaResponse>>> ObtenerTodosAsync()
        {
            try
            {
                var caracteristicas = await _context.Caracteristicas
                    .Where(c => c.Activo)
                    .Select(c => new CaracteristicaResponse
                    {
                        CaracteristicaId = c.CaracteristicaId,
                        Nombre = c.Nombre,
                        Icono = c.Icono,
                        Activo = c.Activo
                    })
                    .ToListAsync();

                return BaseResponse<List<CaracteristicaResponse>>.Ok(caracteristicas, "Características obtenidas exitosamente.");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CaracteristicaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
