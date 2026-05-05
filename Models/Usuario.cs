using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? EstaActivo { get; set; }

    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();

    public virtual ICollection<HistorialEstadoHabitacion> HistorialEstadoHabitacions { get; set; } = new List<HistorialEstadoHabitacion>();

    public virtual CatRolUsuario? IdRolNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
