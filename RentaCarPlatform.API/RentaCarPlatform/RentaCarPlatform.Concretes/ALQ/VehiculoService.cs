using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RentaCarPlatform.Interfaces.ALQ;
using RentaCarPlatform.Models.ALQ;
using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;

namespace RentaCarPlatform.Concretes.ALQ
{
    public class VehiculoService : IVehiculoService
    {
        private readonly RentaCarPlatformContext context;

        public VehiculoService(RentaCarPlatformContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponse<VehiculoResponse>> ActualizarAsync(ActualizarVehiculoRequest request)
        {
            try
            {
                var vehiculo = await context.Vehiculos
                    .Include(v => v.VehiculoCaracteristicas)
                    .FirstOrDefaultAsync(v => v.VehiculoId == request.VehiculoId);

                if (vehiculo is null)
                    return BaseResponse<VehiculoResponse>.NotFound("Vehículo no encontrado.");

                var placaNormalizada = request.Placa.Trim().ToUpper();

                var existePlaca = await context.Vehiculos
                    .AnyAsync(v => v.Placa.ToUpper() == placaNormalizada && v.VehiculoId != request.VehiculoId);

                if (existePlaca)
                    return BaseResponse<VehiculoResponse>.Conflict("La placa ya está registrada en otro vehículo.");

                vehiculo.ModeloId = request.ModeloId;
                vehiculo.TipoVehiculoId = request.TipoVehiculoId;
                vehiculo.EstadoVehiculoId = request.EstadoVehiculoId;
                vehiculo.Anio = request.Anio;
                vehiculo.Placa = placaNormalizada;
                vehiculo.Color = request.Color.Trim();
                vehiculo.Transmision = request.Transmision;
                vehiculo.Combustible = request.Combustible;
                vehiculo.CapacidadPasajeros = request.CapacidadPasajeros;
                vehiculo.PrecioDiario = request.PrecioDiario;
                vehiculo.Descripcion = request.Descripcion?.Trim();

                context.VehiculoCaracteristicas.RemoveRange(vehiculo.VehiculoCaracteristicas);

                if (request.CaracteristicaIds.Count > 0)
                {
                    var caracteristicasValidas = await context.Caracteristicas
                        .Where(c => request.CaracteristicaIds.Contains(c.CaracteristicaId) && c.Activo)
                        .Select(c => c.CaracteristicaId)
                        .ToListAsync();

                    var nuevasRelaciones = caracteristicasValidas
                        .Select(cId => new VehiculoCaracteristicas
                        {
                            VehiculoId = vehiculo.VehiculoId,
                            CaracteristicaId = cId
                        });

                    context.VehiculoCaracteristicas.AddRange(nuevasRelaciones);
                }

                await context.SaveChangesAsync();

                return await ObtenerPorIdAsync(vehiculo.VehiculoId);
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<VehiculoResponse>.Fail($"Error de base de datos al actualizar el vehículo: {ex.InnerException?.Message ?? ex.Message}");

            }
            catch (Exception ex)
            {
                return BaseResponse<VehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<VehiculoResponse>> CrearAsync(CrearVehiculoRequest request, int creadoPorUsuarioId)
        {
            try
            {
                var placaNormalizada = request.Placa.Trim().ToUpper();

                var placaDuplicada = await context.Vehiculos.AnyAsync(v => v.Placa.ToUpper() == placaNormalizada);

                if (placaDuplicada)
                    return BaseResponse<VehiculoResponse>.Conflict($"Ya existe un vehículo con la placa '{request.Placa}'.");

                var modeloExiste = await context.Modelos.AnyAsync(m => m.ModeloId == request.ModeloId && m.Activo);

                if (!modeloExiste)
                    return BaseResponse<VehiculoResponse>.BadRequest($"El ModeloId '{request.ModeloId}' no existe.");

                var tipoVehiculoExiste = await context.TiposVehiculos.AnyAsync(t => t.TipoVehiculoId == request.TipoVehiculoId && t.Activo);

                if (!tipoVehiculoExiste)
                    return BaseResponse<VehiculoResponse>.BadRequest($"El TipoVehiculoId '{request.TipoVehiculoId}' no existe.");

                var estadoVehiculoExiste = await context.EstadosVehiculos.AnyAsync(e => e.EstadoVehiculoId == request.EstadoVehiculoId && e.Activo);

                if (!estadoVehiculoExiste)
                    return BaseResponse<VehiculoResponse>.BadRequest($"El EstadoVehiculoId '{request.EstadoVehiculoId}' no existe.");

                var vehiculo = new Vehiculo
                {
                    ModeloId = request.ModeloId,
                    TipoVehiculoId = request.TipoVehiculoId,
                    EstadoVehiculoId = request.EstadoVehiculoId,
                    CreadoPorUsuarioId = creadoPorUsuarioId,
                    Anio = request.Anio,
                    Placa = placaNormalizada,
                    Color = request.Color.Trim(),
                    Transmision = request.Transmision,
                    Combustible = request.Combustible,
                    CapacidadPasajeros = request.CapacidadPasajeros,
                    PrecioDiario = request.PrecioDiario,
                    Descripcion = request.Descripcion?.Trim(),
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                };

                context.Vehiculos.Add(vehiculo);
                await context.SaveChangesAsync();

                if (request.CaracteristicaIds.Count > 0)
                {
                    var caracteristicasValidas = await context.Caracteristicas
                        .Where(c => request.CaracteristicaIds.Contains(c.CaracteristicaId) && c.Activo)
                        .Select(c => c.CaracteristicaId)
                        .ToListAsync();

                    var relaciones = caracteristicasValidas
                        .Select(cId => new VehiculoCaracteristicas
                        {
                            VehiculoId = vehiculo.VehiculoId,
                            CaracteristicaId = cId
                        });

                    context.VehiculoCaracteristicas.AddRange(relaciones);
                    await context.SaveChangesAsync();
                }

                return await ObtenerPorIdAsync(vehiculo.VehiculoId);

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<VehiculoResponse>.Fail($"Error de base de datos al crear el vehículo: {ex.InnerException?.Message ?? ex.Message}");

            }
            catch (Exception ex)
            {
                return BaseResponse<VehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DesactivarAsync(int vehiculoId)
        {
            try
            {

                var vehiculo = await context.Vehiculos
                    .Include(v => v.EstadoVehiculo)
                    .FirstOrDefaultAsync(v => v.VehiculoId == vehiculoId);

                if (vehiculo is null)
                    return BaseResponse<bool>.NotFound("Vehículo no encontrado.");

                if (vehiculo.EstadoVehiculo.Nombre == "Rentado")
                    return BaseResponse<bool>.BadRequest("No se puede desactivar un vehículo que está actualmente rentado.");

                var tieneReservasActivas = await context.Reservas
                   .AnyAsync(r => r.VehiculoId == vehiculoId &&
                                  (r.Estado == "Pendiente" || r.Estado == "Confirmada" || r.Estado == "En curso"));

                if (tieneReservasActivas)
                    return BaseResponse<bool>.BadRequest("No se puede desactivar el vehículo porque tiene reservas activas.");

                var estadoInactivo = await context.EstadosVehiculos
                    .FirstOrDefaultAsync(e => e.Nombre == "Inactivo");

                if (estadoInactivo is null)
                    return BaseResponse<bool>.Fail("No se encontró el estado 'Inactivo' en el sistema.");

                vehiculo.EstadoVehiculoId = estadoInactivo.EstadoVehiculoId;

                await context.SaveChangesAsync();

                return BaseResponse<bool>.Ok(true, "Vehículo desactivado correctamente.");
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse<bool>.Fail($"Error de base de datos al desactivar el vehículo: {ex.InnerException?.Message ?? ex.Message}");

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<PagedResponse<VehiculoListaResponse>>> ObtenerCatalogoAsync(FiltroVehiculoRequest filtro)
        {
            try
            {
                var query = context.Vehiculos
                    .Include(v => v.Modelo).ThenInclude(m => m.Marca)
                    .Include(v => v.TipoVehiculo)
                    .Include(v => v.EstadoVehiculo)
                    .Include(v => v.VehiculoImagenes)
                    .Where(v => v.EstadoVehiculo.Nombre == "Disponible")
                    .AsQueryable();

                query = AplicarFiltros(query, filtro);

                var totalRegistros = await query.CountAsync();

                var items = await query
                    .OrderBy(v => v.Modelo.Marca.Nombre)
                    .ThenBy(v => v.Modelo.Nombre)
                    .Skip((filtro.Pagina - 1) * filtro.TamanoPagina)
                    .Take(filtro.TamanoPagina)
                    .Select(v => new VehiculoListaResponse
                    {
                        VehiculoId = v.VehiculoId,
                        Marca = v.Modelo.Marca.Nombre,
                        Modelo = v.Modelo.Nombre,
                        TipoVehiculo = v.TipoVehiculo.Nombre,
                        EstadoNombre = v.EstadoVehiculo.Nombre,
                        EstadoColorHex = v.EstadoVehiculo.ColorHex ?? string.Empty,
                        Anio = v.Anio,
                        Color = v.Color,
                        Transmision = v.Transmision,
                        Combustible = v.Combustible,
                        CapacidadPasajeros = v.CapacidadPasajeros,
                        PrecioDiario = v.PrecioDiario,
                        ImagenPrincipal = v.VehiculoImagenes
                                            .Where(i => i.EsPrincipal)
                                            .Select(i => i.Urlimagen)
                                            .FirstOrDefault()
                    })
                    .ToListAsync();

                var paged = new PagedResponse<VehiculoListaResponse>
                {
                    Items = items,
                    TotalRegistros = totalRegistros,
                    Pagina = filtro.Pagina,
                    TamanoPagina = filtro.TamanoPagina
                };
                
                return BaseResponse<PagedResponse<VehiculoListaResponse>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedResponse<VehiculoListaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<VehiculoResponse>> ObtenerPorIdAsync(int vehiculoId)
        {
            try
            {
                var vehiculo = await context.Vehiculos
                    .Include(v => v.Modelo).ThenInclude(m => m.Marca)
                    .Include(v => v.TipoVehiculo)
                    .Include(v => v.EstadoVehiculo)
                    .Include(v => v.VehiculoCaracteristicas).ThenInclude(vc => vc.Caracteristica)
                    .Include(v => v.VehiculoImagenes)
                    .Where(v => v.VehiculoId == vehiculoId)
                    .Select(v => new VehiculoResponse
                    {
                        VehiculoId = v.VehiculoId,
                        ModeloId = v.ModeloId,
                        Marca = v.Modelo.Marca.Nombre,
                        Modelo = v.Modelo.Nombre,
                        TipoVehiculoId = v.TipoVehiculoId,
                        TipoVehiculo = v.TipoVehiculo.Nombre,
                        EstadoVehiculoId = v.EstadoVehiculoId,
                        EstadoNombre = v.EstadoVehiculo.Nombre,
                        EstadoColorHex = v.EstadoVehiculo.ColorHex ?? string.Empty,
                        Anio = v.Anio,
                        Placa = v.Placa,
                        Color = v.Color,
                        Transmision = v.Transmision,
                        Combustible = v.Combustible,
                        CapacidadPasajeros = v.CapacidadPasajeros,
                        PrecioDiario = v.PrecioDiario,
                        Descripcion = v.Descripcion,
                        FechaCreacion = v.FechaCreacion,
                        FechaActualizacion = v.FechaActualizacion,
                        Caracteristicas = v.VehiculoCaracteristicas
                                             .Select(vc => new CaracteristicaResponse
                                             {
                                                 CaracteristicaId = vc.Caracteristica.CaracteristicaId,
                                                 Nombre = vc.Caracteristica.Nombre,
                                                 Icono = vc.Caracteristica.Icono,
                                                 Activo = vc.Caracteristica.Activo
                                             }).ToList(),
                        Imagenes = v.VehiculoImagenes
                                             .OrderBy(i => i.Orden)
                                             .Select(i => new VehiculoImagenResponse
                                             {
                                                 ImagenId = i.ImagenId,
                                                 UrlImagen = i.Urlimagen,
                                                 EsPrincipal = i.EsPrincipal,
                                                 Orden = i.Orden
                                             }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if(vehiculo is null)
                    return BaseResponse<VehiculoResponse>.NotFound("Vehículo no encontrado.");

                return BaseResponse<VehiculoResponse>.Ok(vehiculo);

            }
            catch (Exception ex)
            {
                return BaseResponse<VehiculoResponse>.Fail($"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<PagedResponse<VehiculoListaResponse>>> ObtenerTodosAsync(FiltroVehiculoRequest filtro)
        {
            try
            {
                var query = context.Vehiculos
                   .Include(v => v.Modelo).ThenInclude(m => m.Marca)
                   .Include(v => v.TipoVehiculo)
                   .Include(v => v.EstadoVehiculo)
                   .Include(v => v.VehiculoImagenes)
                   .AsQueryable();

                query = AplicarFiltros(query, filtro);

                var totalRegistros = await query.CountAsync();

                var items = await query
                    .OrderBy(v => v.Modelo.Marca.Nombre)
                    .ThenBy(v => v.Modelo.Nombre)
                    .Skip((filtro.Pagina - 1) * filtro.TamanoPagina)
                    .Take(filtro.TamanoPagina)
                    .Select(v => new VehiculoListaResponse
                    {
                        VehiculoId = v.VehiculoId,
                        Marca = v.Modelo.Marca.Nombre,
                        Modelo = v.Modelo.Nombre,
                        TipoVehiculo = v.TipoVehiculo.Nombre,
                        EstadoNombre = v.EstadoVehiculo.Nombre,
                        EstadoColorHex = v.EstadoVehiculo.ColorHex ?? string.Empty,
                        Anio = v.Anio,
                        Color = v.Color,
                        Transmision = v.Transmision,
                        Combustible = v.Combustible,
                        CapacidadPasajeros = v.CapacidadPasajeros,
                        PrecioDiario = v.PrecioDiario,
                        ImagenPrincipal = v.VehiculoImagenes
                                             .Where(i => i.EsPrincipal)
                                             .Select(i => i.Urlimagen)
                                             .FirstOrDefault()
                    })
                    .ToListAsync();

                var paged = new PagedResponse<VehiculoListaResponse>
                {
                    Items = items,
                    TotalRegistros = totalRegistros,
                    Pagina = filtro.Pagina,
                    TamanoPagina = filtro.TamanoPagina
                };

                return BaseResponse<PagedResponse<VehiculoListaResponse>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedResponse<VehiculoListaResponse>>.Fail($"Internal Server Error: {ex.Message}");
            }
        }






        private static IQueryable<Vehiculo> AplicarFiltros(IQueryable<Vehiculo> query, FiltroVehiculoRequest filtro)
        {
            // Búsqueda libre: Marca o Modelo contiene el texto
            if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
            {
                var texto = filtro.Busqueda.Trim().ToLower();
                query = query.Where(v =>
                    v.Modelo.Nombre.ToLower().Contains(texto) ||
                    v.Modelo.Marca.Nombre.ToLower().Contains(texto) ||
                    v.Placa.ToLower().Contains(texto));
            }

            if (filtro.MarcaId.HasValue)
                query = query.Where(v => v.Modelo.MarcaId == filtro.MarcaId.Value);

            if (filtro.ModeloId.HasValue)
                query = query.Where(v => v.ModeloId == filtro.ModeloId.Value);

            if (filtro.TipoVehiculoId.HasValue)
                query = query.Where(v => v.TipoVehiculoId == filtro.TipoVehiculoId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Transmision))
                query = query.Where(v => v.Transmision == filtro.Transmision);

            if (!string.IsNullOrWhiteSpace(filtro.Combustible))
                query = query.Where(v => v.Combustible == filtro.Combustible);

            if (filtro.CapacidadPasajeros.HasValue)
                query = query.Where(v => v.CapacidadPasajeros >= filtro.CapacidadPasajeros.Value);

            if (filtro.AnioDesde.HasValue)
                query = query.Where(v => v.Anio >= filtro.AnioDesde.Value);

            if (filtro.AnioHasta.HasValue)
                query = query.Where(v => v.Anio <= filtro.AnioHasta.Value);

            if (filtro.PrecioMinimo.HasValue)
                query = query.Where(v => v.PrecioDiario >= filtro.PrecioMinimo.Value);

            if (filtro.PrecioMaximo.HasValue)
                query = query.Where(v => v.PrecioDiario <= filtro.PrecioMaximo.Value);

            return query;
        }
    }
}
