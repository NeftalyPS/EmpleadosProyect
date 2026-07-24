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
    [Route("api/departamentos")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DepartamentosApiController : ControllerBase
    {
        private readonly EmpleadosDbContext _context;
        private readonly IMapper _mapper;

        public DepartamentosApiController(EmpleadosDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartamentoDto>>> GetTodos()
        {
            var lista = await _context.Departamentos.ToListAsync();
            return Ok(_mapper.Map<List<DepartamentoDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoDto>> GetPorId(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();
            return Ok(_mapper.Map<DepartamentoDto>(dep));
        }

        [HttpPost]
        public async Task<ActionResult<DepartamentoDto>> Crear(DepartamentoDto dto)
        {
            var dep = _mapper.Map<Departamento>(dto);
            _context.Departamentos.Add(dep);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPorId), new { id = dep.Id }, _mapper.Map<DepartamentoDto>(dep));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, DepartamentoDto dto)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();
            dep.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var dep = await _context.Departamentos.Include(d => d.Empleados).FirstOrDefaultAsync(d => d.Id == id);
            if (dep == null) return NotFound();
            if (dep.Empleados.Any())
                return BadRequest(new { message = "No se puede eliminar un departamento con empleados asociados" });

            _context.Departamentos.Remove(dep);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}