using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Dominio.Entidades;

namespace ApiFinanzas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        return await _context.Categorias.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetById(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult> Post(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Categoria categoria)
    {
        if (id != categoria.Id) return BadRequest();

        var existe = await _context.Categorias.AnyAsync(c => c.Id == id);
        if (!existe) return NotFound();

        _context.Update(categoria);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
