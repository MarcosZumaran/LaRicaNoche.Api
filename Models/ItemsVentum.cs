using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class ItemsVentum
{
    public int IdItem { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
