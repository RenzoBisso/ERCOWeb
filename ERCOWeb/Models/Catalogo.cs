using System;

namespace ERCOWeb.Models;

public partial class Catalogo
{
    public int IdCatalogo { get; set; }


    public string Tipo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }

    public string? Pdf { get; set; }

    public DateTime FechaActualizacion { get; set; } = DateTime.Now;
}