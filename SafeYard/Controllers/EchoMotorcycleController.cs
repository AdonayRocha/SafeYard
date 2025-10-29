using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;

namespace SafeYard.Controllers
{
    [Route("api/echomotorcycles")]
    [ApiController]
    public class EchoMotorcycleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EchoMotorcycleController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os registros da tabela TB_ECHO_MOTORCYCLE.
        /// </summary>
        /// <returns>Lista de EchoMotorcycle</returns>
        [HttpGet(Name = "GetEchoMotorcycles")]
        [ProducesResponseType(typeof(IEnumerable<EchoMotorcycle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<EchoMotorcycle>>> GetEchoMotorcycles()
        {
            var items = await _context.TB_ECHO_MOTORCYCLE.ToListAsync();
            if (items.Count == 0) return NoContent();
            return Ok(items);
        }
    }
}