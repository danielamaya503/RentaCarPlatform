using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RentaCarPlatform.Interfaces;
using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentaCarPlatform.Concretes
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerarToken(Usuario usuario, string rolNombre)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub,    usuario.UsuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,  usuario.Email),
                new Claim("name",                         usuario.NombreUsuario),  
                new Claim("role",                         rolNombre),           
                new Claim("usuarioId",                    usuario.UsuarioId.ToString()),
                new Claim("rolId",                        usuario.RolId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,    Guid.NewGuid().ToString())
            };

            var expiracion = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
