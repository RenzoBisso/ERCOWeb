using System;
using System.Collections.Generic;

namespace ERCOWeb.Models;

public partial class Promo
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string Pdf { get; set; } = null!;

    public string Imagen { get; set; } = null!;
}
