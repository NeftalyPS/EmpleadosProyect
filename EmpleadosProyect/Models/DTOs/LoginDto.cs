using System.ComponentModel.DataAnnotations;

namespace EmpleadosApp.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }
    }
}