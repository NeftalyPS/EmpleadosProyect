using EmpleadosApp.Data;
using EmpleadosApp.Models.DTOs;
using EmpleadosApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmpleadosApp.Controllers.Api
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly EmpleadosDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthApiController(EmpleadosDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Username == dto.Username);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            var (token, expira) = _tokenService.GenerarToken(usuario);
            return Ok(new TokenResponseDto { Token = token, ExpiraEn = expira });
        }
    }
}