using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;

[Route("api/patios")]
[ApiController]
public class PatiosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatiosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
    {
        return await _context.Patios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patio>> GetPatio(int id)
    {
        var patio = await _context.Patios.FindAsync(id);
        return patio != null ? Ok(patio) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Patio>> PostPatio([FromBody] Patio patio)
    {
        _context.Patios.Add(patio);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, patio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatio(int id, [FromBody] Patio patio)
    {
        if (id != patio.Id) return BadRequest();

        _context.Entry(patio).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatio(int id)
    {
        var patio = await _context.Patios.FindAsync(id);
        if (patio == null) return NotFound();

        _context.Patios.Remove(patio);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
