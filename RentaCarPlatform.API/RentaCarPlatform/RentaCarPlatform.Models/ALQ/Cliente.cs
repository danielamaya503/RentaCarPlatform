using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Dui { get; set; }

    public string Telefono { get; set; } = null!;

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; }

    public int CreadoPorUsuarioId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Usuario CreadoPorUsuario { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
