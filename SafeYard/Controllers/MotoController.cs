using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;
using Swashbuckle.AspNetCore.Filters;

namespace SafeYard.Controllers  
{
    [Route("api/motos")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private HateoasLinkBuilder LinkBuilder => new HateoasLinkBuilder(Url);

        public MotoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todas as motos com paginação e filtro opcional por marca.</summary>
        /// <param name="marca">Marca a filtrar (opcional)</param>
        /// <param name="paging">Parâmetros de paginação (page, pageSize)</param>
        [HttpGet(Name = "GetMotos")]
        [ProducesResponseType(typeof(PagedResult<Resource<Moto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<Resource<Moto>>>> GetMotos([FromQuery] string? marca, [FromQuery] PagingParameters paging)
        {
            var query = _context.Motos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(marca))
                query = query.Where(m => m.Marca == marca);

            var total = await query.CountAsync();
            if (total == 0)
                return NoContent();

            var items = await query
                .OrderBy(m => m.Id)
                .Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

            var resources = items.Select(m =>
            {
                var res = new Resource<Moto>(m);
                res.Links.AddRange(LinkBuilder.ForMoto(m.Id));
                return res;
            }).ToList();

            var result = new PagedResult<Resource<Moto>>(resources, total, paging.Page, paging.PageSize);
            result.Links.AddRange(LinkBuilder.ForCollection("GetMotos", result.Page, result.PageSize, result.TotalPages));
            return Ok(result);
        }

        /// <summary>Retorna uma moto específica pelo ID.</summary>
        [HttpGet("{id}", Name = "GetMotoById")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Resource<Moto>>> GetMoto(int id)
        {
            var moto = await _context.Motos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (moto == null) return NotFound();

            var res = new Resource<Moto>(moto);
            res.Links.AddRange(LinkBuilder.ForMoto(id));
            return Ok(res);
        }

        /// <summary>Retorna motos com ano maior ou igual ao parâmetro informado.</summary>
        [HttpGet("ano", Name = "GetMotosPorAno")]
        [ProducesResponseType(typeof(IEnumerable<Moto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotosPorAno([FromQuery] int? minAno)
        {
            var motos = minAno.HasValue
                ? await _context.Motos.AsNoTracking().Where(m => m.Ano >= minAno.Value).ToListAsync()
                : await _context.Motos.AsNoTracking().ToListAsync();

            return motos.Count > 0 ? Ok(motos) : NoContent();
        }

        /// <summary>Cadastra uma nova moto.</summary>
        [HttpPost(Name = "CreateMoto")]
        [SwaggerRequestExample(typeof(Moto), typeof(SafeYard.Models.Examples.MotoRequestExample))]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Moto>> PostMoto([FromBody] Moto moto)
        {
            if (moto == null || string.IsNullOrWhiteSpace(moto.Modelo))
                return BadRequest("Dados inválidos!");

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetMotoById", new { id = moto.Id }, moto);
        }

        /// <summary>Atualiza uma moto.</summary>
        [HttpPut("{id}", Name = "UpdateMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMoto(int id, [FromBody] Moto moto)
        {
            if (id != moto.Id) return BadRequest("ID inválido!");

            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>Exclui uma moto.</summary>
        [HttpDelete("{id}", Name = "DeleteMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}