using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;

namespace SafeYard.Controllers
{
    [ApiController]
    [Route("api/echo-motorcycles")]
    public class EchoMotorcycleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EchoMotorcycleController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna os registros de EchoMotorcycle (últimos 100 por data/hora).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EchoMotorcycle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<EchoMotorcycle>>> Get()
        {
            var items = await _context.TB_ECHO_MOTORCYCLE
                .AsNoTracking()
                .OrderByDescending(e => e.Data)
                .ThenByDescending(e => e.Hora)
                .Take(100)
                .ToListAsync();

            return items.Count > 0 ? Ok(items) : NoContent();
        }
    }
}