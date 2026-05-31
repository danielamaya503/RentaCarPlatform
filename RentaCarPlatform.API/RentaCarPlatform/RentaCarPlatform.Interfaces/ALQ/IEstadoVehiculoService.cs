using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface IEstadoVehiculoService
    {
        Task<BaseResponse<List<EstadoVehiculoResponse>>> ObtenerTodosAsync();
        Task<BaseResponse<EstadoVehiculoResponse>> ObtenerPorIdAsync(int estadoVehiculoId);
        Task<BaseResponse<EstadoVehiculoResponse>> CrearAsync(CrearEstadoVehiculoRequest request);
        Task<BaseResponse<EstadoVehiculoResponse>> ActualizarAsync(ActualizarEstadoVehiculoRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int estadoVehiculoId);
    }
}
