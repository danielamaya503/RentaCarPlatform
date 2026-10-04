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
    public class VehiculoCaracteristicaService : IVehiculoCaracteristicaService
    {
        private readonly RentaCarPlatformContext context;

        public VehiculoCaracteristicaService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<List<VehiculoCaracteristicaResponse>>> AsignarAsync(AsignarCaracteristicasRequest request)
        {
            try
            {
                var vehiculo = await context.Vehiculos
                    .Include(v => v.VehiculoCaracteristicas)
                    .FirstOrDefaultAsync(v => v.VehiculoId == request.VehiculoId);

                if(vehiculo is null)
                    return BaseResponse<List<VehiculoCaracteristicaResponse>>.NotFound($"No se encontró el vehículo con ID {request.VehiculoId}.");

                var idsUnicos = request.CaracteristicaIds.Distinct().ToList();

                var caracteristicasExistentes = await context.Caracteristicas
                  .Where(c => idsUnicos.Contains(c.CaracteristicaId))
                  .ToListAsync();

                if (caracteristicasExistentes.Count != idsUnicos.Count)
                {
                    var noEncontrados = idsUnicos.Except(caracteristicasExistentes.Select(c => c.CaracteristicaId));
                    return BaseResponse<List<VehiculoCaracteristicaResponse>>.NotFound(
                        $"No se encontraron las siguientes características: {string.Join(", ", noEncontrados)}.");
                }

                var yaAsignadas = vehiculo.VehiculoCaracteristicas.Select(vc => vc.CaracteristicaId).ToHashSet();
                var nuevasAsignaciones = idsUnicos.Where(id => !yaAsignadas.Contains(id)).ToList();

                if (!nuevasAsignaciones.Any())
                    return BaseResponse<List<VehiculoCaracteristicaResponse>>.BadRequest(
                        "Todas las características enviadas ya están asignadas a este vehículo.");

                foreach (var caracteristicaId in nuevasAsignaciones)
                {
                    context.VehiculoCaracteristicas.Add(new VehiculoCaracteristicas
                    {
                        VehiculoId = request.VehiculoId,
                        CaracteristicaId = caracteristicaId
                    });
                }

                await context.SaveChangesAsync();

                var response = await context.VehiculoCaracteristicas
                    .Include(vc => vc.Caracteristica)
                    .Where(vc => vc.VehiculoId == request.VehiculoId)
                    .Select(vc => new VehiculoCaracteristicaResponse
                    {
                        VehiculoId = vc.VehiculoId,
                        CaracteristicaId = vc.CaracteristicaId,
                        CaracteristicaNombre = vc.Caracteristica.Nombre
                    })
                    .ToListAsync();

                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Created(response,
                    $"Se asignaron {nuevasAsignaciones.Count} característica(s) al vehículo {request.VehiculoId}.");
            }
            catch(DbUpdateException ex) 
            {
                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Fail($"Error de base de datos al actualizar la característica: {ex.Message}");
            }
            catch (Exception ex) 
            {
                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<VehiculoCaracteristicaResponse>>> ObtenerPorVehiculoAsync(int vehiculoId)
        {
            try
            {
                var existe = await context.Vehiculos.AnyAsync(v => v.VehiculoId == vehiculoId);

                if (!existe)
                    return BaseResponse<List<VehiculoCaracteristicaResponse>>.NotFound($"No se encontró el vehículo con ID {vehiculoId}.");

                var caracteristicas = await context.VehiculoCaracteristicas
                    .Include(vc => vc.Caracteristica)
                    .Where(vc => vc.VehiculoId == vehiculoId)
                    .Select(vc => new VehiculoCaracteristicaResponse
                    {
                        VehiculoId = vc.VehiculoId,
                        CaracteristicaId = vc.CaracteristicaId,
                        CaracteristicaNombre = vc.Caracteristica.Nombre
                    })
                    .ToListAsync();

                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Ok(caracteristicas);


            }
            catch (Exception ex)
            {
                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<VehiculoCaracteristicaResponse>>> ReemplazarTodasAsync(ReemplazarCaracteristicasRequest request)
        {
            try
            {
                var vehiculo = await context.Vehiculos
                   .Include(v => v.VehiculoCaracteristicas)
                   .FirstOrDefaultAsync(v => v.VehiculoId == request.VehiculoId);

                if (vehiculo is null)
                    return BaseResponse<List<VehiculoCaracteristicaResponse>>.NotFound($"No se encontró el vehículo con ID {request.VehiculoId}.");

                var idsUnicos = request.CaracteristicaIds.Distinct().ToList();

                if (idsUnicos.Any())
                {
                    var caracteristicasExistentes = await context.Caracteristicas
                        .Where(c => idsUnicos.Contains(c.CaracteristicaId))
                        .ToListAsync();

                    if (caracteristicasExistentes.Count != idsUnicos.Count)
                    {
                        var noEncontrados = idsUnicos.Except(caracteristicasExistentes.Select(c => c.CaracteristicaId));
                        return BaseResponse<List<VehiculoCaracteristicaResponse>>.NotFound(
                            $"No se encontraron las siguientes características: {string.Join(", ", noEncontrados)}.");
                    }
                }

                context.VehiculoCaracteristicas.RemoveRange(vehiculo.VehiculoCaracteristicas);

                var nuevasAsignaciones = idsUnicos.Select(id => new VehiculoCaracteristicas
                {
                    VehiculoId = request.VehiculoId,
                    CaracteristicaId = id
                });

                context.VehiculoCaracteristicas.AddRange(nuevasAsignaciones);

                await context.SaveChangesAsync();

                var response = await context.VehiculoCaracteristicas
                    .Include(vc => vc.Caracteristica)
                    .Where(vc => vc.VehiculoId == request.VehiculoId)
                    .Select(vc => new VehiculoCaracteristicaResponse
                    {
                        VehiculoId = vc.VehiculoId,
                        CaracteristicaId = vc.CaracteristicaId,
                        CaracteristicaNombre = vc.Caracteristica.Nombre
                    })
                    .ToListAsync();

                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Ok(response,
                    $"Se sincronizaron las características del vehículo {request.VehiculoId}. Total actual: {response.Count}.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Fail($"Error de base de datos al actualizar la característica: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<VehiculoCaracteristicaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> RemoverAsync(RemoverCaracteristicasRequest request)
        {
            try
            {

                var idsUnicos = request.CaracteristicaIds.Distinct().ToList();

                var asignacionesExistentes = await context.VehiculoCaracteristicas
                    .Where(vc => vc.VehiculoId == request.VehiculoId && idsUnicos.Contains(vc.CaracteristicaId))
                    .ToListAsync();

                if (!asignacionesExistentes.Any())
                    return BaseResponse<bool>.NotFound(
                        "No se encontraron asignaciones de características para remover con los datos enviados.");

                context.VehiculoCaracteristicas.RemoveRange(asignacionesExistentes);

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true,
                    $"Se removieron {asignacionesExistentes.Count} característica(s) del vehículo {request.VehiculoId}.");

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al actualizar la característica: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
