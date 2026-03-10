using System.ComponentModel.DataAnnotations;

namespace WKClientsApi.Models
{
    public class Cliente
    {
        [Required]
        [RegularExpression(@"^[0-9]{8}[A-Z]$", ErrorMessage = "DNI con formato incorrecto")]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Apellidos obligatorios")]
        [StringLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [CustomValidation(typeof(ClienteValidator), "ValidateFechaNacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email con formato incorrecto")]
        public string Email { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9]{9,15}$", ErrorMessage = "Telefono con formato incorrecto")]
        public string Telefono { get; set; } = string.Empty;
    }
}
