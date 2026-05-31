using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface ITipoVehiculoService
    {
        Task<BaseResponse<List<TipoVehiculoResponse>>> ObtenerTodosAsync();
        Task<BaseResponse<TipoVehiculoResponse>> ObtenerPorIdAsync(int tipoVehiculoId);
        Task<BaseResponse<TipoVehiculoResponse>> CrearAsync(CrearTipoVehiculoRequest request);
        Task<BaseResponse<TipoVehiculoResponse>> ActualizarAsync(ActualizarTipoVehiculoRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int tipoVehiculoId);
    }
}
