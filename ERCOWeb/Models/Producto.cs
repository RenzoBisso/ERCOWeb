using System;
using System.Collections.Generic;

namespace ERCOWeb.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public int IdCategoria { get; set; }

    public int IdMarca { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;
}
