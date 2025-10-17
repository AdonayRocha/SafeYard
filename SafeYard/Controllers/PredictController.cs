using Microsoft.AspNetCore.Mvc;
using SafeYard.Services;
using SafeYard.Models;

namespace SafeYard.Controllers
{
    [ApiController]
    [Route("api/v1/motos")]
    public class PredictController : ControllerBase
    {
        private readonly RoboflowService _roboflowService;

        public PredictController(RoboflowService roboflowService)
        {
            _roboflowService = roboflowService;
        }

        /// <summary>
        /// Realiza a predição de motos a partir de uma imagem.
        /// </summary>
        [HttpPost("predict")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Predict([FromForm] PredictImageUpload upload)
        {
            if (upload?.Image == null)
                return BadRequest("Arquivo não encontrado.");

            using var ms = new MemoryStream();
            await upload.Image.CopyToAsync(ms);
            var imageBytes = ms.ToArray();

            int motos = await _roboflowService.DetectMotos(imageBytes);

            return Ok(new { motosDetectadas = motos });
        }
    }
}