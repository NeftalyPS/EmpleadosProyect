using AutoMapper;
using EmpleadosApp.Data;
using EmpleadosApp.Models.DTOs;
using EmpleadosApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosApp.Controllers.Api
{
    [ApiController]
    [Route("api/empleados")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EmpleadosApiController : ControllerBase
    {
        private readonly EmpleadosDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long TamañoMaximoBytes = 5 * 1024 * 1024;

        public EmpleadosApiController(EmpleadosDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }


        [HttpGet]
        public async Task<ActionResult<PagedResult<EmpleadoDto>>> GetEmpleados(
            int page = 1, int pageSize = 10, string? nombre = null,
            string? apellido = null, DateTime? fechaContratacion = null)
        {
            var query = _context.Empleados.Include(e => e.Departamento).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(e => e.Nombre.Contains(nombre));
            if (!string.IsNullOrWhiteSpace(apellido))
                query = query.Where(e => e.Apellido.Contains(apellido));
            if (fechaContratacion.HasValue)
                query = query.Where(e => e.FechaContratacion.Date == fechaContratacion.Value.Date);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<EmpleadoDto>
            {
                Items = _mapper.Map<List<EmpleadoDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.Include(e => e.Departamento)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null) return NotFound();
            return Ok(_mapper.Map<EmpleadoDto>(empleado));
        }


        [HttpPost]
        public async Task<ActionResult<EmpleadoDto>> CrearEmpleado(EmpleadoCreateDto dto)
        {
            if (!await _context.Departamentos.AnyAsync(d => d.Id == dto.DepartamentoId))
                return BadRequest(new { message = "Departamento no válido" });

            var empleado = _mapper.Map<Empleado>(dto);
            empleado.Estado = EstadoEmpleado.Activo;

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.Id }, _mapper.Map<EmpleadoDto>(empleado));
        }

   
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, EmpleadoUpdateDto dto)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (empleado.Estado == EstadoEmpleado.Despedido)
                return BadRequest(new { message = "Un empleado despedido no puede ser modificado" });

            _mapper.Map(dto, empleado);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (empleado.Estado == EstadoEmpleado.Despedido)
                return BadRequest(new { message = "No se puede desactivar a un empleado despedido" });

            empleado.Estado = empleado.Estado == EstadoEmpleado.Activo
                ? EstadoEmpleado.Inactivo
                : EstadoEmpleado.Activo;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}/despedir")]
        public async Task<IActionResult> Despedir(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (empleado.Estado == EstadoEmpleado.Despedido)
                return BadRequest(new { message = "El empleado ya está despedido" });

            empleado.Estado = EstadoEmpleado.Despedido;
            empleado.FechaTerminacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/foto")]
        public async Task<IActionResult> SubirFoto(int id, IFormFile foto)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (foto == null || foto.Length == 0) return BadRequest(new { message = "Archivo inválido" });
            if (!ArchivoEsValido(foto, out var error)) return BadRequest(new { message = error });

            var carpeta = Path.Combine(_env.WebRootPath, "uploads", "empleados");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
            var rutaFisica = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            empleado.FotoRuta = $"/uploads/empleados/{nombreArchivo}";
            await _context.SaveChangesAsync();

            return Ok(new { fotoRuta = empleado.FotoRuta });
        }

        private bool ArchivoEsValido(IFormFile foto, out string error)
        {
            error = string.Empty;
            var extension = Path.GetExtension(foto.FileName).ToLowerInvariant();

            if (!ExtensionesPermitidas.Contains(extension))
            {
                error = "Solo se permiten imágenes JPG, PNG o GIF.";
                return false;
            }
            if (foto.Length > TamañoMaximoBytes)
            {
                error = "El archivo no debe superar los 5 MB.";
                return false;
            }
            return true;
        }
    }
}