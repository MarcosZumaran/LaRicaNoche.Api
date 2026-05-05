using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class Estancia
{
    public int IdEstancia { get; set; }

    public int? IdReserva { get; set; }

    public int? IdHabitacion { get; set; }

    public int? IdClienteTitular { get; set; }

    public DateTime FechaCheckin { get; set; }

    public DateTime FechaCheckoutPrevista { get; set; }

    public DateTime? FechaCheckoutReal { get; set; }

    public decimal MontoTotal { get; set; }

    public string? Estado { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Huespede> Huespedes { get; set; } = new List<Huespede>();

    public virtual Cliente? IdClienteTitularNavigation { get; set; }

    public virtual Habitacione? IdHabitacionNavigation { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual ICollection<ItemsEstancium> ItemsEstancia { get; set; } = new List<ItemsEstancium>();
}
