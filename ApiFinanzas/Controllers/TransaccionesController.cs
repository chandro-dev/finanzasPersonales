using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
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
        return await _context.Transacciones
            .Include(t => t.Categoria)
            .Include(t => t.Cuenta)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Post(Transaccion transaccion)
    {
        var cuenta = await _context.Cuentas.FindAsync(transaccion.CuentaId);
        var categoria = await _context.Categorias.FindAsync(transaccion.CategoriaId);

        if (cuenta is null || categoria is null)
            return BadRequest("Cuenta o categoría no válida.");

        // Ajustar el saldo de la cuenta
        cuenta.SaldoInicial += categoria.EsIngreso ? transaccion.Monto : -transaccion.Monto;

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

        if (transaccion is null) return NotFound();

        // Revertir el saldo
        if (transaccion.Cuenta is not null && transaccion.Categoria is not null)
        {
            transaccion.Cuenta.SaldoInicial -= transaccion.Categoria.EsIngreso
                ? transaccion.Monto
                : -transaccion.Monto;
        }

        _context.Transacciones.Remove(transaccion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
