using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;

namespace SafeYard.Controllers
{
    [Route("api/patios")]
    [ApiController]
    public class PatiosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private HateoasLinkBuilder LinkBuilder => new HateoasLinkBuilder(Url);

        public PatiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna pátios com paginação.</summary>
        [HttpGet(Name = "GetPatios")]
        [ProducesResponseType(typeof(PagedResult<Resource<Patio>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<Resource<Patio>>>> GetPatios([FromQuery] PagingParameters paging)
        {
            var query = _context.Patios.AsNoTracking().OrderBy(p => p.Id);
            var total = await query.CountAsync();
            if (total == 0) return NoContent();

            var items = await query
                .Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

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
            var patio = await _context.Patios.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (patio == null) return NotFound();

            var res = new Resource<Patio>(patio);
            res.Links.AddRange(LinkBuilder.ForPatio(id));
            return Ok(res);
        }

        /// <summary>Cria um novo pátio.</summary>
        [HttpPost(Name = "CreatePatio")]
        [ProducesResponseType(typeof(Patio), StatusCodes.Status201Created)]
        public async Task<ActionResult<Patio>> PostPatio([FromBody] Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetPatioById", new { id = patio.Id }, patio);
        }

        /// <summary>Atualiza um pátio.</summary>
        [HttpPut("{id}", Name = "UpdatePatio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPatio(int id, [FromBody] Patio patio)
        {
            if (id != patio.Id) return BadRequest("ID inválido!");

            _context.Entry(patio).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Exclui um pátio.</summary>
        [HttpDelete("{id}", Name = "DeletePatio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return NotFound();

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}