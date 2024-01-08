using System;
using System.Collections.Generic;

namespace TodoManagementAPI.Models;

public partial class EstadoTarea
{
    public int IdEstado { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
