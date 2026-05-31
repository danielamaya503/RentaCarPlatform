using Microsoft.EntityFrameworkCore;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.Models.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentaCarPlatform.Concretes.ALQ
{
    public class ModeloService : IModeloService
    {
        private readonly RentaCarPlatformContext context;

        public ModeloService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<ModeloResponse>> ActualizarAsync(ActualizarModeloRequest request)
        {
            try
            {
                var existe = await context.Modelos.FindAsync(request.ModeloId);

                if (existe is null) 
                    return BaseResponse<ModeloResponse>.NotFound($"No se encontró el modelo con ID {request.ModeloId} para actualizar.");

                var marcaExiste = await context.Marcas
                   .AnyAsync(m => m.MarcaId == request.MarcaId && m.Activo);

                if(!marcaExiste)
                    return BaseResponse<ModeloResponse>.Conflict("La marca especificada no existe o está inactiva.");

                var duplicado = await context.Modelos
                    .AnyAsync(m =>
                        m.ModeloId != request.ModeloId &&
                        m.MarcaId == request.MarcaId &&
                        m.Nombre == request.Nombre.Trim());

                if (duplicado)
                    return BaseResponse<ModeloResponse>.Conflict($"Ya existe un modelo con el nombre {request.Nombre} para la marca {request.MarcaId}.");
            
                existe.MarcaId = request.MarcaId;
                existe.Nombre = request.Nombre.Trim();
                existe.Activo = request.Activo;

                await context.SaveChangesAsync();

                var response = await context.Modelos
                    .Include(m => m.Marca)
                    .Where(m => m.ModeloId == existe.ModeloId)
                    .Select(m => new ModeloResponse
                    {
                        ModeloId = m.ModeloId,
                        MarcaId = m.MarcaId,
                        MarcaNombre = m.Marca.Nombre,
                        Nombre = m.Nombre,
                        Activo = m.Activo
                    })
                    .FirstAsync();

                return BaseResponse<ModeloResponse>.Ok(response, "Modelo actualizado exitosamente.");
            }
            catch (DbUpdateException db)
            {
                return BaseResponse<ModeloResponse>.Fail($"Error de base de datos al actualizar el modelo: {db.InnerException?.Message ?? db.Message}");

            }
            catch (Exception ex)
            {
                return BaseResponse<ModeloResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<ModeloResponse>> CrearAsync(CrearModeloRequest request)
        {
            try
            {
                var marcaExiste = await context.Marcas
                   .AnyAsync(m => m.MarcaId == request.MarcaId && m.Activo);    

                if (!marcaExiste)
                    return BaseResponse<ModeloResponse>.NotFound("La marca especificada no existe o está inactiva.");

                var existeModelo = await context.Modelos
                    .AnyAsync(m => m.MarcaId == request.MarcaId && m.Nombre == request.Nombre.Trim());

                if (existeModelo)
                    return BaseResponse<ModeloResponse>.Conflict($"Ya existe un modelo con el nombre {request.Nombre} para la marca {request.MarcaId}.");

                var nuevoModelo = new Modelo
                {
                    MarcaId = request.MarcaId,
                    Nombre = request.Nombre,
                    Activo = request.Activo,
                };

                context.Modelos.Add(nuevoModelo);

                await context.SaveChangesAsync();

                var response = await context.Modelos
                    .Include(m => m.Marca)
                    .Where(m => m.ModeloId == nuevoModelo.ModeloId)
                    .Select(m => new ModeloResponse
                    {
                        ModeloId = m.ModeloId,
                        MarcaId = m.MarcaId,
                        MarcaNombre = m.Marca.Nombre,
                        Nombre = m.Nombre,
                        Activo = m.Activo
                    })
                    .FirstAsync();

                return BaseResponse<ModeloResponse>.Created(response, "Modelo creado exitosamente.");
            }
            catch (DbUpdateException db)
            {
                return BaseResponse<ModeloResponse>.Fail($"Error de base de datos al crear el modelo: {db.InnerException?.Message ?? db.Message}");

            }
            catch (Exception ex)
            {
                return BaseResponse<ModeloResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DesactivarAsync(int modeloId)
        {
            try
            {
                var modelo = await context.Modelos.FindAsync(modeloId);

                if (modelo is null)
                    return BaseResponse<bool>.NotFound($"No se encontró el modelo con ID {modeloId} para desactivar.");

                var tieneVehiculosActivos = await context.Vehiculos
                   .AnyAsync(v => v.ModeloId == modeloId);

                if (tieneVehiculosActivos)
                    return BaseResponse<bool>.Conflict("No se puede desactivar el modelo porque tiene vehículos activos asociados.");


                modelo.Activo = false;

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Modelo desactivado exitosamente.");
            }
            catch (DbUpdateException db)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar el modelo: {db.InnerException?.Message ?? db.Message}");

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<ModeloResponse>> ObtenerPorIdAsync(int modeloId)
        {
            try
            {
                var modelos = await context.Modelos
                 .Include(m => m.Marca)
                 .Where(m => m.ModeloId == modeloId)
                 .Select(m => new ModeloResponse
                 {
                     ModeloId = m.ModeloId,
                     MarcaId = m.MarcaId,
                     MarcaNombre = m.Marca.Nombre,
                     Nombre = m.Nombre,
                     Activo = m.Activo
                 })
                 .FirstOrDefaultAsync();
                
                if (modelos is null)
                    return BaseResponse<ModeloResponse>.NotFound($"No se encontró el modelo con ID {modeloId}.");

                return BaseResponse<ModeloResponse>.Ok(modelos);
            }
            catch (Exception ex)
            {
                return BaseResponse<ModeloResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
          

        public async Task<BaseResponse<List<ModeloResponse>>> ObtenerPorMarcaIdAsync(int marcaId)
        {
            try
            {
               var marcaExiste = await context.Marcas.AnyAsync(m => m.MarcaId == marcaId);

                if(!marcaExiste)
                    return BaseResponse<List<ModeloResponse>>.NotFound("La marca especificada no existe.");

                var modelos = await context.Modelos
                   .Include(m => m.Marca)
                   .Where(m => m.MarcaId == marcaId && m.Activo)
                   .OrderBy(m => m.Nombre)
                   .Select(m => new ModeloResponse
                   {
                       ModeloId = m.ModeloId,
                       MarcaId = m.MarcaId,
                       MarcaNombre = m.Marca.Nombre,
                       Nombre = m.Nombre,
                       Activo = m.Activo
                   })
                   .ToListAsync();

                return BaseResponse<List<ModeloResponse>>.Ok(modelos, "Modelos obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<ModeloResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<ModeloResponse>>> ObtenerTodosAsync()
        {
            try
            {
                var modelos = await context.Modelos
                  .Include(m => m.Marca)
                  .Where(m => m.Activo)
                  .OrderBy(m => m.Marca.Nombre)
                  .ThenBy(m => m.Nombre)
                  .Select(m => new ModeloResponse
                  {
                      ModeloId = m.ModeloId,
                      MarcaId = m.MarcaId,
                      MarcaNombre = m.Marca.Nombre,
                      Nombre = m.Nombre,
                      Activo = m.Activo
                  })
                  .ToListAsync();

                return BaseResponse<List<ModeloResponse>>.Ok(modelos, "Modelos obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<ModeloResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
