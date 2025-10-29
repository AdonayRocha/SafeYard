using Microsoft.AspNetCore.Mvc;
using SafeYard.Services;
using System;
using System.Globalization;
using System.Linq;

namespace SafeYard.Controllers
{
    [ApiController]
    [Route("api/motorcycle-ml-prediction")]
    public class MotorcycleMlPredictionController : ControllerBase
    {
        private readonly MotorcycleMlPredictionService _service;

        public MotorcycleMlPredictionController(MotorcycleMlPredictionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna a previsão de quantidade de motos para uma data/hora (ML.NET).
        /// </summary>
        /// <param name="data">Data (yyyy-MM-dd, dd/MM/yyyy, yyyyMMdd)</param>
        /// <param name="hora">Hora (HH:mm, HHmm, Hmm, H/HH)</param>
        [HttpGet]
        [ProducesResponseType(typeof(float), 200)]
        [ProducesResponseType(400)]
        public ActionResult<float> Predict([FromQuery] string data, [FromQuery] string hora)
        {
            if (!TryParseDate(data, out var dt))
                return BadRequest("Parâmetro 'data' inválido. Use 'yyyy-MM-dd', 'dd/MM/yyyy' ou 'yyyyMMdd'.");

            if (!TryParseHoraToDecimal(hora, out var horaDecimal))
                return BadRequest("Parâmetro 'hora' inválido. Use 'HH:mm' (ex.: 13:00) ou 'HHmm' (ex.: 1300).");

            float qtd = _service.Predict(horaDecimal, dt.Day, dt.Month, dt.Year);
            return Ok(qtd);
        }

        private static bool TryParseDate(string data, out DateTime date)
        {
            date = default;
            if (string.IsNullOrWhiteSpace(data)) return false;

            var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "yyyyMMdd" };
            return DateTime.TryParseExact(
                data,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal,
                out date
            );
        }

        private static bool TryParseHoraToDecimal(string hora, out float horaDecimal)
        {
            horaDecimal = 0f;
            if (string.IsNullOrWhiteSpace(hora)) return false;

            if (TimeSpan.TryParse(hora, out var ts))
            {
                horaDecimal = ts.Hours + ts.Minutes / 60f;
                return true;
            }

            var digits = new string(hora.Where(char.IsDigit).ToArray());

            if (digits.Length == 4)
            {
                if (int.TryParse(digits[..2], out var h) && int.TryParse(digits[2..], out var m)
                    && h >= 0 && h < 24 && m >= 0 && m < 60)
                {
                    horaDecimal = h + m / 60f;
                    return true;
                }
            }
            else if (digits.Length == 3)
            {
                if (int.TryParse(digits[..1], out var h) && int.TryParse(digits[1..], out var m)
                    && h >= 0 && h < 24 && m >= 0 && m < 60)
                {
                    horaDecimal = h + m / 60f;
                    return true;
                }
            }
            else if (digits.Length > 0 && digits.Length <= 2)
            {
                if (int.TryParse(digits, out var h) && h >= 0 && h < 24)
                {
                    horaDecimal = h;
                    return true;
                }
            }

            return false;
        }
    }
}