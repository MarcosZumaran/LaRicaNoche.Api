using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Nacionalidad { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaVerificacionReniec { get; set; }

    public virtual ICollection<Estancia> Estancia { get; set; } = new List<Estancia>();

    public virtual ICollection<Huespede> Huespedes { get; set; } = new List<Huespede>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual CatTipoDocumento TipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
