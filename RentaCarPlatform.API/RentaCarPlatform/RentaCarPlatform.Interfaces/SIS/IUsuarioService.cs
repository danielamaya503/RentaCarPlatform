using RentaCarPlatform.ViewModels.SIS.Request;
using RentaCarPlatform.ViewModels.SIS.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.SIS
{
    public interface IUsuarioService
    {
        Task<BaseResponse<List<UsuarioResponse>>> ObtenerTodosAsync();
        Task<BaseResponse<UsuarioResponse>> ObtenerPorIdAsync(int usuarioId);
        Task<BaseResponse<UsuarioResponse>> CrearAsync(CrearUsuarioRequest request);
        Task<BaseResponse<UsuarioResponse>> ActualizarAsync(ActualizarUsuarioRequest request);
        Task<BaseResponse<bool>> CambiarContrasenaAsync(CambiarContrasenaRequest request);
        Task<BaseResponse<bool>> DesactivarAsync(int usuarioId);
    }
}
