using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarPlatform.Models.ALQ;

[Table("VehiculoCaracteristicas", Schema = "ALQ")]
public class VehiculoCaracteristicas
{
    public int VehiculoId { get; set; }
    public int CaracteristicaId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
    public virtual Caracteristica Caracteristica { get; set; } = null!;
}
