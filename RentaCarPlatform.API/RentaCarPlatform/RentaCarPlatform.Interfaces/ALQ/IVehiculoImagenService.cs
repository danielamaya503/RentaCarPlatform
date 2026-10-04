using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface IVehiculoImagenService
    {
        Task<BaseResponse<List<VehiculoImagenResponse2>>> ObtenerPorVehiculoAsync(int vehiculoId);
        Task<BaseResponse<List<VehiculoImagenResponse2>>> CrearAsync(CrearVehiculoImagenesRequest request);
        Task<BaseResponse<List<VehiculoImagenResponse2>>> ActualizarAsync(ActualizarVehiculoImagenRequest request);
        Task<BaseResponse<bool>> EliminarAsync(int imagenId);
        Task<BaseResponse<bool>> EstablecerComoPrincipalAsync(int imagenId);
        Task<BaseResponse<bool>> ReordenarAsync(ReordenarVehiculoImagenesRequest request);
    }
}
