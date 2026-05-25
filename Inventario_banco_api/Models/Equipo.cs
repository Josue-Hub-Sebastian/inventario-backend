namespace Inventario_banco_api.Models
{
    public class Equipo
    {
        public int Id { get; set; }
        public string CodigoPatrimonial { get; set; }
        public string TipoEquipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string EstadoEquipo { get; set; }
        public string Ubicacion { get; set; }
        public string UsuarioAsignado { get; set; }
        public DateTime FechaAdquisicion { get; set; }
    }
}
