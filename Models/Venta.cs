using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? IdCliente { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaVenta { get; set; }

    public decimal Total { get; set; }

    public string? MetodoPago { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<ItemsVentum> ItemsVenta { get; set; } = new List<ItemsVentum>();

    public virtual CatMetodoPago? MetodoPagoNavigation { get; set; }
}
