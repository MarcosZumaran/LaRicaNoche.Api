using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int? IdCliente { get; set; }

    public int? IdHabitacion { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime FechaEntradaPrevista { get; set; }

    public DateTime FechaSalidaPrevista { get; set; }

    public decimal MontoTotal { get; set; }

    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    public virtual ICollection<Estancia> Estancia { get; set; } = new List<Estancia>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Habitacione? IdHabitacionNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
