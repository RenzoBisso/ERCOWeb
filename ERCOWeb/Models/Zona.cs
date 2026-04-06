using System;
using System.Collections.Generic;

namespace ERCOWeb.Models;

public partial class Zona
{
    public int IdZona { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }
}
