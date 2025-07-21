using Dominio.Entidades;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace ApiFinanzas.Servicios;

public class ProgramacionService : IProgramacionService
{
    private readonly AppDbContext _context;

    public ProgramacionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarProgramacionesActivas()
    {
        var programaciones = await _context.ProgramacionAutomaticas
            .Where(p => p.Activa)
            .ToListAsync();

        foreach (var p in programaciones)
        {
            RecurringJob.AddOrUpdate<IProgramacionService>(
                $"programacion-{p.Id}",
                service => service.EjecutarProgramacion(p.Id),
                p.CronExpresion);
        }
    }

    public async Task EjecutarProgramacion(int programacionId)
    {
        var p = await _context.ProgramacionAutomaticas
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

        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();
    }
}
