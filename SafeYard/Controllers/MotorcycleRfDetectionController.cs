using Microsoft.AspNetCore.Mvc;
using SafeYard.Services;
using System.Threading.Tasks;

namespace SafeYard.Controllers
{
    [ApiController]
    [Route("api/motorcycle-rf-detection")]
    [ApiExplorerSettings(GroupName = "MotorcycleRfDetection")]
    public class MotorcycleRfDetectionController : ControllerBase
    {
        private readonly MotorcycleRfDetectionService _service;

        public MotorcycleRfDetectionController(MotorcycleRfDetectionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Detecta motos em uma imagem usando Roboflow.
        /// </summary>
        /// <param name="image">Arquivo de imagem</param>
        /// <returns>Quantidade de motos detectadas</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> Detect([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Imagem não enviada.");

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var qtd = await _service.DetectMotos(ms.ToArray());
            return Ok(qtd);
        }
    }
}