using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class Habitacione
{
    public int IdHabitacion { get; set; }

    public string NumeroHabitacion { get; set; } = null!;

    public int? Piso { get; set; }

    public string? Descripcion { get; set; }

    public int IdTipo { get; set; }

    public decimal PrecioNoche { get; set; }

    public int? IdEstado { get; set; }

    public DateTime? FechaUltimoCambio { get; set; }

    public int? UsuarioCambio { get; set; }

    public virtual ICollection<Estancia> Estancia { get; set; } = new List<Estancia>();

    public virtual ICollection<HistorialEstadoHabitacion> HistorialEstadoHabitacions { get; set; } = new List<HistorialEstadoHabitacion>();

    public virtual CatEstadoHabitacion? IdEstadoNavigation { get; set; }

    public virtual TiposHabitacion IdTipoNavigation { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Usuario? UsuarioCambioNavigation { get; set; }
}
