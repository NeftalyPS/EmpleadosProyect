using EmpleadosApp.Data;
using EmpleadosApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosApp.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadosDbContext _context;
        private readonly IWebHostEnvironment _env;
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long TamañoMaximoBytes = 5 * 1024 * 1024; // 5 MB

        public EmpleadosController(EmpleadosDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1, string? nombre = null, string? apellido = null, DateTime? fechaContratacion = null)
        {
            const int pageSize = 10;
            var query = _context.Empleados.Include(e => e.Departamento).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre)) query = query.Where(e => e.Nombre.Contains(nombre));
            if (!string.IsNullOrWhiteSpace(apellido)) query = query.Where(e => e.Apellido.Contains(apellido));
            if (fechaContratacion.HasValue) query = query.Where(e => e.FechaContratacion.Date == fechaContratacion.Value.Date);

            ViewBag.Nombre = nombre;
            ViewBag.Apellido = apellido;
            ViewBag.FechaContratacion = fechaContratacion?.ToString("yyyy-MM-dd");
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);

            var empleados = await query.OrderBy(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _context.Empleados.Include(e => e.Departamento).FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado, IFormFile? foto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
                return View(empleado);
            }

            if (foto != null && foto.Length > 0)
            {
                if (!ArchivoEsValido(foto, out var error))
                {
                    ModelState.AddModelError("", error);
                    ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
                    return View(empleado);
                }
                empleado.FotoRuta = await GuardarFotoAsync(foto);
            }

            empleado.Estado = EstadoEmpleado.Activo;
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (empleado.Estado == EstadoEmpleado.Despedido)
                return RedirectToAction(nameof(Details), new { id });

            ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado, IFormFile? foto)
        {
            if (id != empleado.Id) return BadRequest();

            var existente = await _context.Empleados.FindAsync(id);
            if (existente == null) return NotFound();
            if (existente.Estado == EstadoEmpleado.Despedido)
                return BadRequest("Un empleado despedido no puede ser modificado");

            if (!ModelState.IsValid)
            {
                ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
                return View(empleado);
            }

            existente.Nombre = empleado.Nombre;
            existente.Apellido = empleado.Apellido;
            existente.DepartamentoId = empleado.DepartamentoId;
            existente.Puesto = empleado.Puesto;
            existente.Salario = empleado.Salario;
            existente.Direccion = empleado.Direccion;
            existente.Telefono = empleado.Telefono;
            existente.Email = empleado.Email;

            if (foto != null && foto.Length > 0)
            {
                if (!ArchivoEsValido(foto, out var error))
                {
                    ModelState.AddModelError("", error);
                    ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
                    return View(empleado);
                }
                existente.FotoRuta = await GuardarFotoAsync(foto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (empleado.Estado != EstadoEmpleado.Despedido)
                empleado.Estado = empleado.Estado == EstadoEmpleado.Activo ? EstadoEmpleado.Inactivo : EstadoEmpleado.Activo;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Despedir(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            if (empleado.Estado != EstadoEmpleado.Despedido)
            {
                empleado.Estado = EstadoEmpleado.Despedido;
                empleado.FechaTerminacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GuardarFotoAsync(IFormFile foto)
        {
            var carpeta = Path.Combine(_env.WebRootPath, "uploads", "empleados");
            Directory.CreateDirectory(carpeta);
            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
            var rutaFisica = Path.Combine(carpeta, nombreArchivo);
            using var stream = new FileStream(rutaFisica, FileMode.Create);
            await foto.CopyToAsync(stream);
            return $"/uploads/empleados/{nombreArchivo}";
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