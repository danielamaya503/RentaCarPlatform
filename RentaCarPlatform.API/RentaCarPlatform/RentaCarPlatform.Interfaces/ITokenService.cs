using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario, string rolNombre);
    }
}
