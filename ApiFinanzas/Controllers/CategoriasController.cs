using Dominio.DTOS;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

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
    public async Task<ActionResult<IEnumerable<CategoriaRespuestaDto>>> Get()
    {
        var categorias = await _context.Categorias.ToListAsync();

        var result = categorias.Select(c => new CategoriaRespuestaDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            EsIngreso = c.EsIngreso
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            EsIngreso = dto.EsIngreso
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();


        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, new CategoriaRespuestaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            EsIngreso= categoria.EsIngreso
            
        });
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetById(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
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
