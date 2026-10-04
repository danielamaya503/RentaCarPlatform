using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface ICaracteristicaService
    {
        Task<BaseResponse<List<CaracteristicaResponse>>> ObtenerTodosAsync();
        Task<BaseResponse<CaracteristicaResponse>> ObtenerPorIdAsync(int caracteristicaId);
        Task<BaseResponse<CaracteristicaResponse>> CrearAsync(CrearCaracteristicaRequest request);
        Task<BaseResponse<CaracteristicaResponse>> ActualizarAsync(ActualizarCaracteristicaRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int caracteristicaId);
    }
}
