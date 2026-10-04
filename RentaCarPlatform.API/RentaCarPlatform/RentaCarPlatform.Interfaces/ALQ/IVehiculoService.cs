using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface IVehiculoService
    {
        // Catálogo público
        Task<BaseResponse<PagedResponse<VehiculoListaResponse>>> ObtenerCatalogoAsync(FiltroVehiculoRequest filtro);

        // Administración
        Task<BaseResponse<PagedResponse<VehiculoListaResponse>>> ObtenerTodosAsync(FiltroVehiculoRequest filtro);
        Task<BaseResponse<VehiculoResponse>> ObtenerPorIdAsync(int vehiculoId);
        Task<BaseResponse<VehiculoResponse>> CrearAsync(CrearVehiculoRequest request, int creadoPorUsuarioId);
        Task<BaseResponse<VehiculoResponse>> ActualizarAsync(ActualizarVehiculoRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int vehiculoId);
    }
}
