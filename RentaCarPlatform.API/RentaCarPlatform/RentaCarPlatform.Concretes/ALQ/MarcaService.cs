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
    public class MarcaService : IMarcaService
    {
        private readonly RentaCarPlatformContext context;

        public MarcaService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<MarcaResponse>> ActualizarAsync(ActualizarMarcaRequest request)
        {
            try
            {
                var marca = await context.Marcas.FindAsync(request.MarcaId);

                if (marca is null)
                    return BaseResponse<MarcaResponse>.Conflict("Marca no encontrada");

                var existe = await context.Marcas
                    .AnyAsync(m => m.Nombre.ToLower() == request.Nombre.ToLower() && 
                                    m.MarcaId != request.MarcaId);
                
                if (existe)
                    return BaseResponse<MarcaResponse>.Conflict("Ya existe una marca con el mismo nombre");
              
                marca.Nombre = request.Nombre;
                marca.Activo = request.Activo;

                await context.SaveChangesAsync();

                var response = new MarcaResponse
                {
                    MarcaId = marca.MarcaId,
                    Nombre = marca.Nombre,
                    Activo = marca.Activo
                };
                return BaseResponse<MarcaResponse>.Ok(response, "Marca actualizada exitosamente");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<MarcaResponse>.Fail($"Error de base de datos al actualizar la marca: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<MarcaResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<MarcaResponse>> CrearAsync(CrearMarcaRequest request)
        {
            try
            {
                var marca = await context.Marcas.FirstOrDefaultAsync(m => m.Nombre.ToLower() == request.Nombre.ToLower());

                if (marca is not null)
                    return BaseResponse<MarcaResponse>.Conflict("La marca ya existe");

                var nuevaMarca = new Marca
                {
                    Nombre = request.Nombre,
                    Activo = request.Activo
                };

                context.Marcas.Add(nuevaMarca);

                await context.SaveChangesAsync();

                var response = new MarcaResponse
                {
                    MarcaId = nuevaMarca.MarcaId,
                    Nombre = nuevaMarca.Nombre,
                    Activo = nuevaMarca.Activo
                };

                return BaseResponse<MarcaResponse>.Created(response, "Marca creada exitosamente");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<MarcaResponse>.Fail($"Error de base de datos al crear la marca: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<MarcaResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DesactivarAsync(int marcaId)
        {
            try
            {
                var marca = await context.Marcas.FindAsync(marcaId);

                if (marca is null)
                    return BaseResponse<bool>.NotFound("Marca no encontrada");

                var modelosAsociados = await context.Modelos.AnyAsync(m => m.MarcaId == marcaId && m.Activo);

                if (modelosAsociados)
                    return BaseResponse<bool>.Conflict("No se puede desactivar la marca porque tiene modelos asociados activos");

                marca.Activo = false;

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Marca desactivada exitosamente");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar la marca: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<MarcaResponse>> ObtenerPorIdAsync(int marcaId)
        {
            try
            {
                var marca = await context.Marcas
                    .Where(m => m.MarcaId == marcaId && m.Activo)
                    .Select(m => new MarcaResponse
                    {
                        MarcaId = m.MarcaId,
                        Nombre = m.Nombre,
                        Activo = m.Activo
                    })
                    .FirstOrDefaultAsync();

                if(marca is null)
                    return BaseResponse<MarcaResponse>.NotFound("Marca no encontrada");

                return BaseResponse<MarcaResponse>.Ok(marca);
            }
            catch (Exception ex)
            {
                return BaseResponse<MarcaResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<MarcaResponse>>> ObtenerTodosAsync()
        {
            try
            {
                var marcas = await context.Marcas
                    .Where(m => m.Activo)
                    .Select(m => new MarcaResponse
                    {
                        MarcaId = m.MarcaId,
                        Nombre = m.Nombre,
                        Activo = m.Activo
                    })
                    .ToListAsync();

                return BaseResponse<List<MarcaResponse>>.Ok(marcas);

            }
            catch (Exception ex)
            {
                return BaseResponse<List<MarcaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
