using Dominio.DTOS;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace ApiFinanzas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransaccionesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransaccionesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaccion>>> Get()
    {
        var transacciones = await _context.Transacciones
            .Include(t => t.Categoria)
            .Include(t => t.Cuenta)
            .ToListAsync();

        return Ok(transacciones);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TransaccionDto dto)
    {
        var cuenta = await _context.Cuentas.FindAsync(dto.CuentaId);
        var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);

        if (cuenta is null || categoria is null)
            return BadRequest("Cuenta o Categoría no válidas.");

        var transaccion = new Transaccion
        {
            Monto = dto.Monto,
            Fecha = dto.Fecha,
            Descripcion = dto.Descripcion,
            CuentaId = dto.CuentaId,
            CategoriaId = dto.CategoriaId,
            EsAutomatica = dto.EsAutomatica
        };

        cuenta.SaldoInicial += categoria.EsIngreso ? dto.Monto : -dto.Monto;

        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = transaccion.Id }, transaccion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var transaccion = await _context.Transacciones
            .Include(t => t.Cuenta)
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaccion is null)
            return NotFound();

        if (transaccion.Cuenta != null && transaccion.Categoria != null)
        {
            // Revertir el saldo
            transaccion.Cuenta.SaldoInicial -= transaccion.Categoria.EsIngreso
                ? transaccion.Monto
                : -transaccion.Monto;
        }

        _context.Transacciones.Remove(transaccion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
