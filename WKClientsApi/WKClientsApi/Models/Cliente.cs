namespace WKClientsApi.Models
{
    public class Cliente
    {
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}
