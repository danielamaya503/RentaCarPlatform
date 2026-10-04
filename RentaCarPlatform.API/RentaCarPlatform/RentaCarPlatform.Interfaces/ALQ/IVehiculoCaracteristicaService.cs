using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface IVehiculoCaracteristicaService
    {
        Task<BaseResponse<List<VehiculoCaracteristicaResponse>>> ObtenerPorVehiculoAsync(int vehiculoId);
        Task<BaseResponse<List<VehiculoCaracteristicaResponse>>> AsignarAsync(AsignarCaracteristicasRequest request);
        Task<BaseResponse<bool>> RemoverAsync(RemoverCaracteristicasRequest request);
        Task<BaseResponse<List<VehiculoCaracteristicaResponse>>> ReemplazarTodasAsync(ReemplazarCaracteristicasRequest request);
    }
}
