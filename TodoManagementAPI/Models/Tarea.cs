using System;
using System.Collections.Generic;

namespace TodoManagementAPI.Models;

public partial class Tarea
{
    public int IdTarea { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstadoTarea { get; set; }

    public int? IdUsuario { get; set; }

    public virtual EstadoTarea? IdEstadoTareaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
