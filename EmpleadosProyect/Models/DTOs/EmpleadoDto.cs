using EmpleadosApp.Models.Entities;

namespace EmpleadosApp.Models.DTOs
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string? FotoRuta { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int DepartamentoId { get; set; }
        public string? DepartamentoNombre { get; set; }
        public string Puesto { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaContratacion { get; set; }
        public DateTime? FechaTerminacion { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EstadoEmpleado Estado { get; set; }
    }
}