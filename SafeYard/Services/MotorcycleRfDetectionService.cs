using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SafeYard.Services;

namespace SafeYard.Services
{
    public class MotorcycleRfDetectionService
    {
        private readonly string _apiKey;
        private readonly string _datasetName;
        private readonly string _datasetVersion;

        public MotorcycleRfDetectionService(IConfiguration config)
        {
            _apiKey = config["Roboflow:ApiKey"];
            _datasetName = config["Roboflow:DatasetName"];
            _datasetVersion = config["Roboflow:DatasetVersion"];
        }

        public async Task<int> DetectMotos(byte[] imageBytes)
        {
            string url = $"https://serverless.roboflow.com/{_datasetName}/{_datasetVersion}?api_key={_apiKey}";
            string base64 = System.Convert.ToBase64String(imageBytes);

            using var client = new HttpClient();
            var content = new StringContent(base64, System.Text.Encoding.ASCII, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return -1;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("predictions", out var predictions))
                return predictions.GetArrayLength();
            return 0;
        }
    }
}