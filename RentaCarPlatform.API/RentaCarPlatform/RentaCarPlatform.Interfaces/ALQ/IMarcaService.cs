using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface IMarcaService
    {
        Task<BaseResponse<List<MarcaResponse>>> ObtenerTodosAsync();
        Task<BaseResponse<MarcaResponse>> ObtenerPorIdAsync(int marcaId);
        Task<BaseResponse<MarcaResponse>> CrearAsync(CrearMarcaRequest request);
        Task<BaseResponse<MarcaResponse>> ActualizarAsync(ActualizarMarcaRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int marcaId);
    }
}
