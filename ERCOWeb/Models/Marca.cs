using System;
using System.Collections.Generic;

namespace ERCOWeb.Models;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public byte[] Imagen { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
