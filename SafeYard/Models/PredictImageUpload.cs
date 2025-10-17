using Microsoft.AspNetCore.Http;

namespace SafeYard.Models
{
    public class PredictImageUpload
    {
        /// <summary>
        /// Arquivo de imagem (campo 'image')
        /// </summary>
        public IFormFile Image { get; set; }
    }
}