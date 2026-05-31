using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class VwVehiculosDisponible
{
    public int VehiculoId { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string TipoVehiculo { get; set; } = null!;

    public int Anio { get; set; }

    public string Placa { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Transmision { get; set; } = null!;

    public string Combustible { get; set; } = null!;

    public int CapacidadPasajeros { get; set; }

    public decimal PrecioDiario { get; set; }

    public string? Descripcion { get; set; }
}
