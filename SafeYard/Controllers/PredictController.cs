using Microsoft.AspNetCore.Mvc;
using SafeYard.Models;
using SafeYard.Services;

namespace SafeYard.Controllers
{
    [ApiController]
    [Route("api/predict")]
    public class PredictController : ControllerBase
    {
        private readonly MotorcycleRfDetectionService _rfService;

        public PredictController(MotorcycleRfDetectionService rfService)
        {
            _rfService = rfService;
        }

        /// <summary>
        /// Detecta motos em uma imagem (Roboflow) e retorna a quantidade detectada.
        /// </summary>
        [HttpPost]
        [ActionName("Predict")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Predict([FromForm] PredictImageUpload upload)
        {
            if (upload?.Image == null || upload.Image.Length == 0)
                return BadRequest("Imagem não enviada.");

            using var ms = new MemoryStream();
            await upload.Image.CopyToAsync(ms);

            var qtd = await _rfService.DetectMotos(ms.ToArray());
            if (qtd < 0)
                return BadRequest("Falha ao processar a imagem.");

            return Ok(qtd);
        }
    }
}