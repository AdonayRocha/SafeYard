using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;

using SafeYard.Models;

/// <summary>
/// Controller responsável pelo gerenciamento de motos na API.
/// Permite criar, consultar, atualizar e excluir registros.
/// </summary>
[Route("api/motos")]
[ApiController]
public class MotoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MotoController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todas as motos cadastradas, filtrando por marca caso necessário.
    /// </summary>
    /// <param name="marca">Nome da marca da moto (QueryParam)</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Moto>>> GetMotos([FromQuery] string? marca)
    {
        var motos = string.IsNullOrEmpty(marca)
            ? await _context.Motos.ToListAsync()
            : await _context.Motos.Where(m => m.Marca == marca).ToListAsync();

        return motos.Count > 0 ? Ok(motos) : NoContent();
    }

    /// <summary>
    /// Retorna uma moto específica pelo ID.
    /// </summary>
    /// <param name="id">ID da moto</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Moto>> GetMoto(int id)
    {
        var moto = await _context.Motos.FindAsync(id);
        return moto != null ? Ok(moto) : NotFound();
    }

    /// <summary>
    /// Retorna todas as motos acima de um ano mínimo especificado.
    /// </summary>
    /// <param name="minAno">Ano mínimo da moto (QueryParam)</param>
    [HttpGet("ano")]
    public async Task<ActionResult<IEnumerable<Moto>>> GetMotosPorAno([FromQuery] int? minAno)
    {
        var motos = minAno.HasValue
            ? await _context.Motos.Where(m => m.Ano >= minAno.Value).ToListAsync()
            : await _context.Motos.ToListAsync();

        return motos.Count > 0 ? Ok(motos) : NoContent();
    }

    /// <summary>
    /// Cadastra uma nova moto na base de dados.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Moto>> PostMoto([FromBody] Moto moto)
    {
        if (moto == null || string.IsNullOrEmpty(moto.Modelo))
            return BadRequest("Dados inválidos!");

        _context.Motos.Add(moto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
    }

    /// <summary>
    /// Atualiza um registro de moto.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMoto(int id, [FromBody] Moto moto)
    {
        if (id != moto.Id) return BadRequest("ID inválido!");

        _context.Entry(moto).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Exclui um registro de moto.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMoto(int id)
    {
        var moto = await _context.Motos.FindAsync(id);
        if (moto == null) return NotFound();

        _context.Motos.Remove(moto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
