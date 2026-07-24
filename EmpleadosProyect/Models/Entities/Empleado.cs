using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpleadosApp.Models.Entities
{
    public class Empleado
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string? FotoRuta { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        public int DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; }

        [Required, StringLength(100)]
        public string Puesto { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,2)")]
        public decimal Salario { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public DateTime FechaContratacion { get; set; }

        public DateTime? FechaTerminacion { get; set; }

        [Required, StringLength(255)]
        public string Direccion { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        public EstadoEmpleado Estado { get; set; } = EstadoEmpleado.Activo;
    }
}