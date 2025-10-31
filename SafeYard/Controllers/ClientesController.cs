using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;
using SafeYard.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace SafeYard.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;
        private HateoasLinkBuilder LinkBuilder => new HateoasLinkBuilder(Url);

        [ActivatorUtilitiesConstructor]
        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        public ClientesController(SafeYard.Data.ApplicationDbContext context)
        {
            _service = new ClienteService(context);
        }

        /// <summary>Retorna todos os clientes com paginação.</summary>
        [HttpGet(Name = "GetClientes")]
        [ProducesResponseType(typeof(PagedResult<Resource<Cliente>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<Resource<Cliente>>>> GetClientes([FromQuery] PagingParameters paging)
        {
            var (items, total) = await _service.GetAsync(paging);
            if (total == 0) return NoContent();

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
            var cliente = await _service.GetByIdAsync(id);
            if (cliente == null) return NotFound();

            var res = new Resource<Cliente>(cliente);
            res.Links.AddRange(LinkBuilder.ForCliente(id));
            return Ok(res);
        }

        /// <summary>Cria um novo cliente.</summary>
        [HttpPost(Name = "CreateCliente")]
        [SwaggerRequestExample(typeof(Cliente), typeof(SafeYard.Models.Examples.ClienteRequestExample))]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            if (cliente == null) return BadRequest("Dados inválidos!");

            var created = await _service.CreateAsync(cliente);
            return CreatedAtRoute("GetClienteById", new { id = created.Id }, created);
        }

        /// <summary>Atualiza um cliente.</summary>
        [HttpPut("{id}", Name = "UpdateCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest("ID inválido!");

            var ok = await _service.UpdateAsync(cliente);
            if (!ok) return NotFound();

            return NoContent();
        }

        /// <summary>Exclui um cliente.</summary>
        [HttpDelete("{id}", Name = "DeleteCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}