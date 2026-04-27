using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class VEstadoHabitacione
{
    public string NumeroHabitacion { get; set; } = null!;

    public string TipoHabitacion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public decimal PrecioNoche { get; set; }

    public DateTime? FechaUltimoCambio { get; set; }
}
