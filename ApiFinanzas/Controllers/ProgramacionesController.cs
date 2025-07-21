using Dominio.DTOS;
using Dominio.Entidades;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace ApiFinanzas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgramacionesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProgramacionesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CrearProgramacion([FromBody] ProgramacionAutomatica programacion)
    {
        // Validar existencia de cuenta y categoría
        var cuenta = await _context.Cuentas.FindAsync(programacion.CuentaId);
        var categoria = await _context.Categorias.FindAsync(programacion.CategoriaId);

        if (cuenta is null || categoria is null)
            return BadRequest("Cuenta o Categoría no válidas");

        _context.ProgramacionAutomaticas.Add(programacion);
        await _context.SaveChangesAsync();

        // Registra el trabajo recurrente en Hangfire
        RecurringJob.AddOrUpdate(
            $"programacion-{programacion.Id}",
            () => EjecutarProgramacionAutomatica(programacion.Id),
            programacion.CronExpresion
        );

        return Ok(programacion);
    }

    [NonAction]
    public async Task EjecutarProgramacionAutomatica(int programacionId)
    {
        using var scope = HttpContext.RequestServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var p = await context.ProgramacionAutomaticas
            .Include(p => p.Cuenta)
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == programacionId);

        if (p is null || !p.Activa) return;

        var transaccion = new Transaccion
        {
            CuentaId = p.CuentaId,
            CategoriaId = p.CategoriaId,
            Monto = p.Monto,
            Fecha = DateTime.UtcNow,
            Descripcion = p.Descripcion,
            EsAutomatica = true
        };

        if (p.Categoria!.EsIngreso)
            p.Cuenta!.SaldoInicial += p.Monto;
        else
            p.Cuenta!.SaldoInicial -= p.Monto;

        context.Transacciones.Add(transaccion);
        await context.SaveChangesAsync();
    }
}