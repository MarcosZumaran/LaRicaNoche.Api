using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class CatTipoComprobante
{
    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
}
