using System.ComponentModel.DataAnnotations;

namespace EmpleadosApp.Models.DTOs
{
    public class EmpleadoCreateDto
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public int DepartamentoId { get; set; }

        [Required, StringLength(100)]
        public string Puesto { get; set; } = string.Empty;

        [Range(0, 9999999)]
        public decimal Salario { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public DateTime FechaContratacion { get; set; }

        [Required, StringLength(255)]
        public string Direccion { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;
    }
}