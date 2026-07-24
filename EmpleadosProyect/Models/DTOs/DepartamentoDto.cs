using System.ComponentModel.DataAnnotations;

namespace EmpleadosApp.Models.DTOs
{
    public class DepartamentoDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}