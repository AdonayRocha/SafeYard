using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services;
using SafeYard.Services.Interfaces;

namespace SafeYard.Controllers
{
    [Route("api/echo-motorcycles")]
    [ApiController]
    public class EchoMotorcyclesController : ControllerBase
    {
        private const int MaxPageSize = 600;

        private readonly IEchoMotorcycleService _service;

        [ActivatorUtilitiesConstructor]
        public EchoMotorcyclesController(IEchoMotorcycleService service)
        {
            _service = service;
        }

        // Construtor alternativo para cenários de teste com DbContext
        public EchoMotorcyclesController(ApplicationDbContext context)
        {
            _service = new EchoMotorcycleService(context);
        }

        /// <summary>Retorna registros de EchoMotorcycle com paginação (máx. 600 por chamada).</summary>
        /// <param name="page">Número da página (1..n)</param>
        /// <param name="pageSize">Tamanho da página (1..600)</param>
        [HttpGet(Name = "GetEchoMotorcycles")]
        [ProducesResponseType(typeof(PagedResult<EchoMotorcycle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResult<EchoMotorcycle>>> Get([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var p = page.GetValueOrDefault(1);
            var s = pageSize.GetValueOrDefault(MaxPageSize);

            if (p < 1) p = 1;
            if (s < 1) s = 1;
            if (s > MaxPageSize) s = MaxPageSize;

            var (items, total) = await _service.GetAsync(p, s);
            if (total == 0) return NoContent();

            var result = new PagedResult<EchoMotorcycle>(items, total, p, s);
            return Ok(result);
        }
    }
}