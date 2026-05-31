using RentaCarPlatform.ViewModels.ALQ.Request;
using RentaCarPlatform.ViewModels.ALQ.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.ALQ
{
    public interface IModeloService
    {
        Task<BaseResponse<List<ModeloResponse>>> ObtenerTodosAsync();
        Task<BaseResponse<ModeloResponse>> ObtenerPorIdAsync(int modeloId);
        Task<BaseResponse<List<ModeloResponse>>> ObtenerPorMarcaIdAsync(int marcaId);
        Task<BaseResponse<ModeloResponse>> CrearAsync(CrearModeloRequest request);
        Task<BaseResponse<ModeloResponse>> ActualizarAsync(ActualizarModeloRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int modeloId);
    }
}
