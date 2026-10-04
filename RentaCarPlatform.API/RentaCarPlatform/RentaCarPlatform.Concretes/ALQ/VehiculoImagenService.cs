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
    public class VehiculoImagenService : IVehiculoImagenService
    {
        private readonly RentaCarPlatformContext _context;

        public VehiculoImagenService(RentaCarPlatformContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<VehiculoImagenResponse2>>> ActualizarAsync(ActualizarVehiculoImagenRequest request)
        {
            try
            {
                if(request.Vehiculos is null || request.Vehiculos.Count == 0)
                    return BaseResponse<List<VehiculoImagenResponse2>>.BadRequest("Debe enviar al menos una imagen para actualizar.");

                var imagenIds = request.Vehiculos.Select(i => i.ImagenId).ToList();

                var imagenesExistentes = await _context.VehiculoImagenes
                   .Include(i => i.Vehiculo)
                       .ThenInclude(v => v.VehiculoImagenes)
                   .Where(i => imagenIds.Contains(i.ImagenId))
                   .ToListAsync();

                if (imagenesExistentes.Count != imagenIds.Count)
                {
                    var idsNoEncontrados = imagenIds.Except(imagenesExistentes.Select(i => i.ImagenId));
                    return BaseResponse<List<VehiculoImagenResponse2>>.NotFound(
                        $"No se encontraron las siguientes imágenes: {string.Join(", ", idsNoEncontrados)}.");
                }



                foreach (var itemImagen in request.Vehiculos)
                {
                    var imagen = imagenesExistentes.First(i => i.ImagenId == itemImagen.ImagenId);

                    imagen.Urlimagen = itemImagen.UrlImagen;
                    imagen.Orden = itemImagen.Orden;

                    if (itemImagen.EsPrincipal && !imagen.EsPrincipal)
                    {
                        foreach (var img in imagen.Vehiculo.VehiculoImagenes)
                            img.EsPrincipal = false;

                        imagen.EsPrincipal = true;
                    }
                    else if (!itemImagen.EsPrincipal && imagen.EsPrincipal)
                    {
                        var tieneOtraPrincipal = imagen.Vehiculo.VehiculoImagenes
                            .Any(i => i.ImagenId != imagen.ImagenId && i.EsPrincipal);

                        if (!tieneOtraPrincipal)
                            return BaseResponse<List<VehiculoImagenResponse2>>.BadRequest(
                                $"No se puede desmarcar la imagen {imagen.ImagenId} como principal sin asignar otra imagen como principal.");

                        imagen.EsPrincipal = false;
                    }
                }

                await _context.SaveChangesAsync();

                var response = imagenesExistentes.Select(imagen => new VehiculoImagenResponse2
                {
                    ImagenId = imagen.ImagenId,
                    VehiculoId = imagen.VehiculoId,
                    UrlImagen = imagen.Urlimagen,
                    EsPrincipal = imagen.EsPrincipal,
                    Orden = imagen.Orden,
                    FechaCreacion = imagen.FechaCreacion
                }).ToList();

                return BaseResponse<List<VehiculoImagenResponse2>>.Ok(response,
                           $"Se actualizaron {response.Count} imágenes correctamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<List<VehiculoImagenResponse2>>.Fail($"Error de base de datos al actualizar la imagen del vehiculo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<VehiculoImagenResponse2>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<VehiculoImagenResponse2>>> CrearAsync(CrearVehiculoImagenesRequest request)
        {
            try
            {
                var vehiculo = await _context.Vehiculos
                    .Include(v => v.VehiculoImagenes)
                    .FirstOrDefaultAsync(v => v.VehiculoId == request.VehiculoId);

                if(vehiculo is null)
                    return BaseResponse<List<VehiculoImagenResponse2>>.NotFound($"No se encontró el vehículo con ID {request.VehiculoId}.");

                var ordenActual = vehiculo.VehiculoImagenes.Any()
                          ? vehiculo.VehiculoImagenes.Max(i => i.Orden)
                          : 0;

                var yaTienePrincipal = vehiculo.VehiculoImagenes.Any(i => i.EsPrincipal);
                var nuevasImagenes = new List<VehiculoImagene>();

                foreach (var item in request.Imagenes)
                { 
                    ordenActual++;
                    var esPrincipal = item.EsPrincipal;

                    // Si esta imagen viene marcada como principal, desmarcar todas las anteriores
                    if (esPrincipal)
                    {
                        foreach (var img in vehiculo.VehiculoImagenes)
                            img.EsPrincipal = false;

                        foreach (var nueva in nuevasImagenes)
                            nueva.EsPrincipal = false;

                        yaTienePrincipal = true;
                    }
                    else if(!yaTienePrincipal)
                    {
                        // Asignar la primera imagen como principal si no hay ninguna asignada
                        esPrincipal = true;
                        yaTienePrincipal = true;
                    }

                    var nuevaImagen = new VehiculoImagene
                    {
                        VehiculoId = request.VehiculoId,
                        Urlimagen = item.UrlImagen,
                        EsPrincipal = esPrincipal,
                        Orden = ordenActual,
                        FechaCreacion = DateTime.UtcNow
                    };

                    nuevasImagenes.Add(nuevaImagen);
                }

                _context.VehiculoImagenes.AddRange(nuevasImagenes);

                await _context.SaveChangesAsync();

                var response = nuevasImagenes.Select(img => new VehiculoImagenResponse2
                {
                    ImagenId = img.ImagenId,
                    VehiculoId = img.VehiculoId,
                    UrlImagen = img.Urlimagen,
                    EsPrincipal = img.EsPrincipal,
                    Orden = img.Orden,
                    FechaCreacion = img.FechaCreacion
                }).ToList();


                return BaseResponse<List<VehiculoImagenResponse2>>.Created(response, $"Imagen del vehículo creada exitosamente con ID {response.First().ImagenId}.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<List<VehiculoImagenResponse2>>.Fail($"Error de base de datos al crear la imagen del vehiculo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<VehiculoImagenResponse2>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> EliminarAsync(int imagenId)
        {
            try
            {
                var imagen = await _context.VehiculoImagenes
                    .FirstOrDefaultAsync(i => i.ImagenId == imagenId);

                if (imagen is null)
                    return BaseResponse<bool>.NotFound($"No se encontró la imagen con ID {imagenId}.");

                var vehiculoId = imagen.VehiculoId;
                var eraPrincipal = imagen.EsPrincipal;

                _context.VehiculoImagenes.Remove(imagen);
                await _context.SaveChangesAsync();


                if (eraPrincipal)
                {
                    var siguiente = await _context.VehiculoImagenes
                       .Where(i => i.VehiculoId == vehiculoId)
                       .OrderBy(i => i.Orden)
                       .FirstOrDefaultAsync();

                    if (siguiente is not null)
                    {
                        siguiente.EsPrincipal = true;
                        await _context.SaveChangesAsync();
                    }
                }
            
                return BaseResponse<bool>.Ok(true, $"Imagen del vehículo eliminada exitosamente con ID {imagenId}.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al eliminar la imagen del vehiculo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> EstablecerComoPrincipalAsync(int imagenId)
        {
            try
            {
                var imagen = await _context.VehiculoImagenes
                   .Include(i => i.Vehiculo)
                       .ThenInclude(v => v.VehiculoImagenes)
                   .FirstOrDefaultAsync(i => i.ImagenId == imagenId);

                if (imagen is null)
                    return BaseResponse<bool>.Fail("Imagen no encontrada.");

                foreach (var img in imagen.Vehiculo.VehiculoImagenes)
                    img.EsPrincipal = false;

                imagen.EsPrincipal = true;

                await _context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Imagen establecida como principal correctamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al establecer la imagen como principal: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<VehiculoImagenResponse2>>> ObtenerPorVehiculoAsync(int vehiculoId)
        {
            try
            {
                var vehiculo = await _context.Vehiculos.AnyAsync(v => v.VehiculoId == vehiculoId);

                if (!vehiculo)
                    return BaseResponse<List<VehiculoImagenResponse2>>.NotFound($"No se encontró el vehículo con ID {vehiculoId}.");
                
               var imagenes = await _context.VehiculoImagenes
                    .Where(vi => vi.VehiculoId == vehiculoId)
                    .OrderBy(vi => vi.Orden)
                    .ThenBy(i  => i.ImagenId)
                    .Select(i => new VehiculoImagenResponse2
                    {
                        ImagenId = i.ImagenId,
                        VehiculoId = i.VehiculoId,
                        UrlImagen = i.Urlimagen,
                        EsPrincipal = i.EsPrincipal,
                        Orden = i.Orden,
                        FechaCreacion = i.FechaCreacion
                    })
                    .ToListAsync();


                return BaseResponse<List<VehiculoImagenResponse2>>.Ok(imagenes, $"Se obtuvieron {imagenes.Count} imagen(es) para el vehículo con ID {vehiculoId}.");

            }
            catch (Exception ex)
            {
                return BaseResponse<List<VehiculoImagenResponse2>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> ReordenarAsync(ReordenarVehiculoImagenesRequest request)
        {
            try
            {

                var imagenes = await _context.VehiculoImagenes
                    .Where(i => i.VehiculoId == request.VehiculoId)
                    .ToListAsync();

                if (!imagenes.Any())
                    return BaseResponse<bool>.Fail("No se encontraron imágenes para el vehículo especificado.");

                var idsExistentes = imagenes.Select(i => i.ImagenId).ToHashSet();
                var idsEnRequest = request.Imagenes.Select(x => x.ImagenId).ToHashSet();

                if (!idsEnRequest.IsSubsetOf(idsExistentes))
                    return BaseResponse<bool>.Fail("La lista de imágenes contiene identificadores que no pertenecen al vehículo.");

                foreach (var item in request.Imagenes)
                {
                    var imagen = imagenes.First(i => i.ImagenId == item.ImagenId);
                    imagen.Orden = item.Orden;
                }

                await _context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Orden de imágenes actualizado correctamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al reordenar las imagenes del vehiculo: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
