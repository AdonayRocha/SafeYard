using Microsoft.ML.Data;

namespace SafeYard.Models.Prediction
{
    public class EchoMotorcycleInput
    {
        // Features
        public float Hora { get; set; }
        public float Dia { get; set; }
        public float Mes { get; set; }
        public float Ano { get; set; }

        // Label
        public float QtdMotos { get; set; }
    }
}