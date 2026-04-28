using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class Huespede
{
    public int IdHuesped { get; set; }

    public int? IdEstancia { get; set; }

    public int? IdCliente { get; set; }

    public bool? EsTitular { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Estancia? IdEstanciaNavigation { get; set; }
}
