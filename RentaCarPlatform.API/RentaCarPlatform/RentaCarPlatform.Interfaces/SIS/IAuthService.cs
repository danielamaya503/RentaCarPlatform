using RentaCarPlatform.ViewModels.SIS.Request;
using RentaCarPlatform.ViewModels.SIS.Response;
using RentaCarPlatform.ViewModels.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces.SIS
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
