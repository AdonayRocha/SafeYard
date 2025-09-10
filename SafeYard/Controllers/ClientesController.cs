using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;

namespace SafeYard.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private HateoasLinkBuilder LinkBuilder => new HateoasLinkBuilder(Url);

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todos os clientes com paginação.</summary>
        [HttpGet(Name = "GetClientes")]
        [ProducesResponseType(typeof(PagedResult<Resource<Cliente>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<Resource<Cliente>>>> GetClientes([FromQuery] PagingParameters paging)
        {
            var query = _context.Clientes.AsNoTracking().OrderBy(c => c.Id);
            var total = await query.CountAsync();

            if (total == 0) return NoContent();

            var items = await query
                .Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

            var resources = items.Select(c =>
            {
                var res = new Resource<Cliente>(c);
                res.Links.AddRange(LinkBuilder.ForCliente(c.Id));
                return res;
            });

            var result = new PagedResult<Resource<Cliente>>(resources, total, paging.Page, paging.PageSize);
            result.Links.AddRange(LinkBuilder.ForCollection("GetClientes", result.Page, result.PageSize, result.TotalPages));
            return Ok(result);
        }

        /// <summary>Retorna um cliente pelo ID.</summary>
        [HttpGet("{id}", Name = "GetClienteById")]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Resource<Cliente>>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null) return NotFound();

            var res = new Resource<Cliente>(cliente);
            res.Links.AddRange(LinkBuilder.ForCliente(id));
            return Ok(res);
        }

        /// <summary>Cria um novo cliente.</summary>
        [HttpPost(Name = "CreateCliente")]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            if (cliente == null) return BadRequest("Dados inválidos!");

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetClienteById", new { id = cliente.Id }, cliente);
        }

        /// <summary>Atualiza um cliente.</summary>
        [HttpPut("{id}", Name = "UpdateCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest("ID inválido!");

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>Exclui um cliente.</summary>
        [HttpDelete("{id}", Name = "DeleteCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}