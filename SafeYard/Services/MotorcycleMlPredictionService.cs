using Microsoft.ML;
using SafeYard.Models.Prediction;
using SafeYard.Data;
using System.Globalization;

namespace SafeYard.Services
{
    public class MotorcycleMlPredictionService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _modelPath;
        private readonly MLContext _mlContext = new();
        private readonly object _sync = new();

        private ITransformer? _model;

        public MotorcycleMlPredictionService(ApplicationDbContext context)
        {
            _context = context;
            _modelPath = Path.Combine(AppContext.BaseDirectory, "motorcycle_ml_model.zip");
        }

        public void TrainModel()
        {
            var records = _context.TB_ECHO_MOTORCYCLE.ToList();
            if (records.Count < 10)
            {
                // Não há dados suficientes para treinar
                _model = null;
                return;
            }

            var data = records.Select(e => new EchoMotorcycleInput
            {
                Hora = ToDecimalHour(e.Hora),
                Dia = e.Data.Day,
                Mes = e.Data.Month,
                Ano = e.Data.Year,
                QtdMotos = e.QtdMotos
            });

            var trainData = _mlContext.Data.LoadFromEnumerable(data);

            var pipeline = _mlContext.Transforms.Concatenate(
                                "Features",
                                nameof(EchoMotorcycleInput.Hora),
                                nameof(EchoMotorcycleInput.Dia),
                                nameof(EchoMotorcycleInput.Mes),
                                nameof(EchoMotorcycleInput.Ano))
                            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                            .Append(_mlContext.Regression.Trainers.Sdca(
                                labelColumnName: nameof(EchoMotorcycleInput.QtdMotos)));

            var model = pipeline.Fit(trainData);

            Directory.CreateDirectory(Path.GetDirectoryName(_modelPath)!);
            _mlContext.Model.Save(model, trainData.Schema, _modelPath);
            _model = model;
        }

        // Predição com fallback se não houver modelo
        public float Predict(float hora, float dia, float mes, float ano)
        {
            EnsureModel();

            if (_model == null)
            {
                var avg = _context.TB_ECHO_MOTORCYCLE.Any()
                    ? _context.TB_ECHO_MOTORCYCLE.Average(x => (double)x.QtdMotos)
                    : 0.0;

                return (float)Math.Max(0, avg);
            }

            using var engine = _mlContext.Model.CreatePredictionEngine<EchoMotorcycleInput, EchoMotorcyclePrediction>(_model);

            var input = new EchoMotorcycleInput
            {
                Hora = hora,
                Dia = dia,
                Mes = mes,
                Ano = ano
            };

            var pred = engine.Predict(input);
            var value = float.IsFinite(pred.Score) ? pred.Score : 0f;

            return MathF.Max(0f, value);
        }

        // Carrega o modelo do disco; se não existir, treina e salva
        private void EnsureModel()
        {
            if (_model != null) return;

            lock (_sync)
            {
                if (_model != null) return;

                try
                {
                    if (File.Exists(_modelPath))
                    {
                        using var fs = File.OpenRead(_modelPath);
                        _model = _mlContext.Model.Load(fs, out _);
                    }
                    else
                    {
                        TrainModel();

                        if (_model == null && File.Exists(_modelPath))
                        {
                            using var fs = File.OpenRead(_modelPath);
                            _model = _mlContext.Model.Load(fs, out _);
                        }
                    }
                }
                catch
                {
                    _model = null;
                }
            }
        }

        private static float ToDecimalHour(string? hora)
        {
            if (string.IsNullOrWhiteSpace(hora)) return 0f;

            if (TimeSpan.TryParse(hora, CultureInfo.InvariantCulture, out var ts))
                return (float)ts.TotalHours;

            var parts = hora.Split(':');
            if (parts.Length >= 2 &&
                float.TryParse(parts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var h) &&
                float.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var m))
            {
                return h + (m / 60f);
            }

            return 0f;
        }
    }
}