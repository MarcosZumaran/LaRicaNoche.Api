using System;
using System.Collections.Generic;

namespace HotelGenericoApi.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? CodigoSunat { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioUnitario { get; set; }

    public string? IdAfectacionIgv { get; set; }

    public int? IdCategoria { get; set; }

    public int? Stock { get; set; }

    public int? StockMinimo { get; set; }

    public string? UnidadMedida { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CatAfectacionIgv? IdAfectacionIgvNavigation { get; set; }

    public virtual CatCategoriaProducto? IdCategoriaNavigation { get; set; }

    public virtual ICollection<ItemsEstancium> ItemsEstancia { get; set; } = new List<ItemsEstancium>();

    public virtual ICollection<ItemsVentum> ItemsVenta { get; set; } = new List<ItemsVentum>();
}
