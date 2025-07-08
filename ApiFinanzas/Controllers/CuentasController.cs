using Microsoft.AspNetCore.Mvc;
using System;
using Persistencia;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanzas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {
        private readonly AppDbContext _context;

        public CuentasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> Get()
        {
            return await _context.Cuentas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetById(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            return cuenta is null ? NotFound() : Ok(cuenta);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cuenta cuenta)
        {
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cuenta.Id }, cuenta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id) return BadRequest();

            var existe = await _context.Cuentas.AnyAsync(c => c.Id == id);
            if (!existe) return NotFound();

            _context.Update(cuenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta is null) return NotFound();

            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
