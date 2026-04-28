using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class CatEstadoSunat
{
    public int Codigo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? DescripcionLarga { get; set; }

    public virtual ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
}
