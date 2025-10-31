using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;
using SafeYard.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace SafeYard.Controllers  
{
    [Route("api/motos")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _service;
        private HateoasLinkBuilder LinkBuilder => new HateoasLinkBuilder(Url);

        [ActivatorUtilitiesConstructor]
        public MotoController(IMotoService service)
        {
            _service = service;
        }

        // Construtor para cenários de teste unitário que instanciam com DbContext
        public MotoController(SafeYard.Data.ApplicationDbContext context)
        {
            _service = new MotoService(context);
        }

        /// <summary>Retorna todas as motos sem paginação.</summary>
        [HttpGet("all", Name = "GetAllMotos")]
        [ProducesResponseType(typeof(IEnumerable<Resource<Moto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Resource<Moto>>>> GetAllMotos()
        {
            var items = await _service.GetAllAsync();
            if (items.Count == 0) return NoContent();

            var resources = items.Select(m =>
            {
                var res = new Resource<Moto>(m);
                res.Links.AddRange(LinkBuilder.ForMoto(m.Id));
                return res;
            }).ToList();

            return Ok(resources);
        }

        /// <summary>Retorna todas as motos com paginação e filtro opcional por marca.</summary>
        /// <param name="marca">Marca a filtrar (opcional)</param>
        /// <param name="paging">Parâmetros de paginação (page, pageSize)</param>
        [HttpGet(Name = "GetMotos")]
        [ProducesResponseType(typeof(PagedResult<Resource<Moto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<Resource<Moto>>>> GetMotos([FromQuery] string? marca, [FromQuery] PagingParameters paging)
        {
            var (items, total) = await _service.GetAsync(marca, paging);
            if (total == 0) return NoContent();

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
            var moto = await _service.GetByIdAsync(id);
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
            var motos = await _service.GetByAnoAsync(minAno);
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

            var created = await _service.CreateAsync(moto);
            return CreatedAtRoute("GetMotoById", new { id = created.Id }, created);
        }

        /// <summary>Atualiza uma moto.</summary>
        [HttpPut("{id}", Name = "UpdateMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMoto(int id, [FromBody] Moto moto)
        {
            if (id != moto.Id) return BadRequest("ID inválido!");

            var ok = await _service.UpdateAsync(moto);
            if (!ok) return NotFound();

            return NoContent();
        }

        /// <summary>Exclui uma moto.</summary>
        [HttpDelete("{id}", Name = "DeleteMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}