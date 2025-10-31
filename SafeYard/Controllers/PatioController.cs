using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;
using SafeYard.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace SafeYard.Controllers
{
    [Route("api/patios")]
    [ApiController]
    public class PatiosController : ControllerBase
    {
        private readonly IPatioService _service;
        private HateoasLinkBuilder LinkBuilder => new HateoasLinkBuilder(Url);

        [ActivatorUtilitiesConstructor]
        public PatiosController(IPatioService service)
        {
            _service = service;
        }

        // Construtor para cenários de teste unitário que instanciam com DbContext
        public PatiosController(SafeYard.Data.ApplicationDbContext context)
        {
            _service = new PatioService(context);
        }

        /// <summary>Retorna pátios com paginação.</summary>
        [HttpGet(Name = "GetPatios")]
        [ProducesResponseType(typeof(PagedResult<Resource<Patio>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<Resource<Patio>>>> GetPatios([FromQuery] PagingParameters paging)
        {
            var (items, total) = await _service.GetAsync(paging);
            if (total == 0) return NoContent();

            var resources = items.Select(p =>
            {
                var res = new Resource<Patio>(p);
                res.Links.AddRange(LinkBuilder.ForPatio(p.Id));
                return res;
            });

            var result = new PagedResult<Resource<Patio>>(resources, total, paging.Page, paging.PageSize);
            result.Links.AddRange(LinkBuilder.ForCollection("GetPatios", result.Page, result.PageSize, result.TotalPages));
            return Ok(result);
        }

        /// <summary>Retorna um pátio por ID.</summary>
        [HttpGet("{id}", Name = "GetPatioById")]
        [ProducesResponseType(typeof(Patio), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Resource<Patio>>> GetPatio(int id)
        {
            var patio = await _service.GetByIdAsync(id);
            if (patio == null) return NotFound();

            var res = new Resource<Patio>(patio);
            res.Links.AddRange(LinkBuilder.ForPatio(id));
            return Ok(res);
        }

        /// <summary>Cria um novo pátio.</summary>
        [HttpPost(Name = "CreatePatio")]
        [SwaggerRequestExample(typeof(Patio), typeof(SafeYard.Models.Examples.PatioRequestExample))]
        [ProducesResponseType(typeof(Patio), StatusCodes.Status201Created)]
        public async Task<ActionResult<Patio>> PostPatio([FromBody] Patio patio)
        {
            var created = await _service.CreateAsync(patio);
            return CreatedAtRoute("GetPatioById", new { id = created.Id }, created);
        }

        /// <summary>Atualiza um pátio.</summary>
        [HttpPut("{id}", Name = "UpdatePatio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPatio(int id, [FromBody] Patio patio)
        {
            if (id != patio.Id) return BadRequest("ID inválido!");

            var ok = await _service.UpdateAsync(patio);
            if (!ok) return NotFound();

            return NoContent();
        }

        /// <summary>Exclui um pátio.</summary>
        [HttpDelete("{id}", Name = "DeletePatio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}