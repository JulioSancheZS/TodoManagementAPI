namespace TodoManagementAPI.DTO
{
    public class TareaDTO
    {
        public int IdTarea { get; set; }

        public string? Titulo { get; set; }

        public string? Descripcion { get; set; }

        public int? IdEstadoTarea { get; set; }

        public int? IdUsuario { get; set; }

        public string? EstadoTarea { get; set; }
    }
}
