using System;
using System.Collections.Generic;
using RentaCarPlatform.Models.ALQ;

namespace RentaCarPlatform.Models.SIS;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public int? CreadoPorUsuarioId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Usuario? CreadoPorUsuario { get; set; }

    public virtual ICollection<DisponibilidadVehiculo> DisponibilidadVehiculos { get; set; } = new List<DisponibilidadVehiculo>();

    public virtual ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();

    public virtual ICollection<HistorialPrecio> HistorialPrecios { get; set; } = new List<HistorialPrecio>();

    public virtual ICollection<Usuario> InverseCreadoPorUsuario { get; set; } = new List<Usuario>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Role Rol { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
