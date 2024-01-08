using System;
using System.Collections.Generic;

namespace TodoManagementAPI.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Usuario1 { get; set; }

    public string? Password { get; set; }

    public string? Correo { get; set; }

    public string? PrimerNombre { get; set; }

    public string? PrimerApellido { get; set; }

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
