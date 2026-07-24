using EmpleadosApp.Data;
using EmpleadosApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosApp.Controllers
{
    [Authorize]
    public class DepartamentosController : Controller
    {
        private readonly EmpleadosDbContext _context;

        public DepartamentosController(EmpleadosDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Departamentos.ToListAsync());
        }

        [Authorize(Roles = "Admin,RRHH")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,RRHH")]
        public async Task<IActionResult> Create(Departamento departamento)
        {
            if (!ModelState.IsValid) return View(departamento);
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,RRHH")]
        public async Task<IActionResult> Edit(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();
            return View(dep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,RRHH")]
        public async Task<IActionResult> Edit(int id, Departamento departamento)
        {
            if (id != departamento.Id) return BadRequest();
            if (!ModelState.IsValid) return View(departamento);

            var existente = await _context.Departamentos.FindAsync(id);
            if (existente == null) return NotFound();
            existente.Nombre = departamento.Nombre;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var dep = await _context.Departamentos.Include(d => d.Empleados).FirstOrDefaultAsync(d => d.Id == id);
            if (dep == null) return NotFound();
            if (dep.Empleados.Any())
            {
                TempData["Error"] = "No se puede eliminar un departamento con empleados asociados.";
                return RedirectToAction(nameof(Index));
            }
            _context.Departamentos.Remove(dep);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}