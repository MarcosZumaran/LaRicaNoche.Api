using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class CatRolUsuario
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
