using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class CatEstadoHabitacion
{
    public int IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool PermiteCheckin { get; set; }

    public bool PermiteCheckout { get; set; }

    public bool EsEstadoFinal { get; set; }

    public string? ColorUi { get; set; }

    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();
}
