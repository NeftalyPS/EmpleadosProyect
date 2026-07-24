using System.ComponentModel.DataAnnotations;

namespace EmpleadosApp.Models.Entities
{
    public class Departamento
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}