using Dominio.DTOS;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;

namespace ApiFinanzas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        public CuentasController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaRespuestaDto>>> Get()
        {
            // await _emailService.EnviarAsync("alejocarreteroballesteros@gmail.com", "Prueba de correo", "<b>Hola!</b> Este es un correo de prueba.");

            var cuentas = await _context.Cuentas.ToListAsync();

            var result = cuentas.Select(c => new CuentaRespuestaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                SaldoInicial = c.SaldoInicial,
                Tipo = c.Tipo
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CuentaDto dto)
        {
            var cuenta = new Cuenta
            {
                Nombre = dto.Nombre,
                SaldoInicial = dto.SaldoInicial,
                Tipo = dto.Tipo
            };

            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cuenta.Id }, new CuentaRespuestaDto
            {
                Id = cuenta.Id,
                Nombre = cuenta.Nombre,
                SaldoInicial = cuenta.SaldoInicial,
                Tipo = cuenta.Tipo
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetById(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            return cuenta is null ? NotFound() : Ok(cuenta);
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
