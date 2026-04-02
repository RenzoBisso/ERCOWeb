using System;
using System.Collections.Generic;

namespace ERCOWeb.Models;

public partial class Sucursal
{
    public int IdSucursal { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public string? Ubicacion { get; set; }

    public bool? Principal { get; set; }
}
