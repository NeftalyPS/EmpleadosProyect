using EmpleadosApp.Models.Entities;

namespace EmpleadosApp.Services
{
    public interface ITokenService
    {
        (string token, DateTime expira) GenerarToken(Usuario usuario);
    }
}