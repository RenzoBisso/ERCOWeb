using System;
using System.Collections.Generic;

namespace ERCOWeb.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Estado { get; set; }


    public Usuario(string nombre, string email, bool estado)
    {
        Nombre = nombre;
        Email = email;
        Estado = estado;
    }
}
