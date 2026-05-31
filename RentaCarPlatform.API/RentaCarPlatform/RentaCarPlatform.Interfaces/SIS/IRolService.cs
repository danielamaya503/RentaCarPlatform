using RentaCarPlatform.ViewModels.SIS.Request;
using RentaCarPlatform.ViewModels.SIS.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.SIS
{
    public interface IRolService
    {
        Task<BaseResponse<List<RolResponse>>> ObtenerTodos();
        Task<BaseResponse<RolResponse>> ObtenerPorId(int rolId);
        Task<BaseResponse<RolResponse>> Crear(CrearRolRequest request);
        Task<BaseResponse<RolResponse>> Actualizar(ActualizarRolRequest request);
        Task<BaseResponse<bool>> Desactivar(int rolId);
    }
}
