using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;

/// <summary>
/// Controller responsável pelo gerenciamento de clientes na API.
/// Permite criar, consultar, atualizar e excluir clientes.
/// </summary>
[Route("api/clientes")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todos os clientes cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return clientes.Count > 0 ? Ok(clientes) : NoContent();
    }

    /// <summary>
    /// Retorna um cliente específico pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return cliente != null ? Ok(cliente) : NotFound();
    }

    /// <summary>
    /// Cria um novo cliente na API.
    /// </summary>
    /// <param name="cliente">Objeto Cliente</param>
    [HttpPost]
    public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
    {
        if (cliente == null)
            return BadRequest("Dados inválidos!");

        // Removida validação de CPF

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    /// <summary>
    /// Atualiza um cliente existente.
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <param name="cliente">Objeto Cliente atualizado</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest("ID inválido!");

        // Removida validação de CPF

        _context.Entry(cliente).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Exclui um cliente da base de dados.
    /// </summary>
    /// <param name="id">ID do cliente</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
