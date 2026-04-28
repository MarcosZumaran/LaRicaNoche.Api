using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class CatAfectacionIgv
{
    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
