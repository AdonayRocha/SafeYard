using System.ComponentModel.DataAnnotations;

namespace SafeYard.Models
{
    /// <summary>
    /// Representa um pátio de armazenamento de motocicletas.
    /// </summary>
    public class Patio
    {
        /// <summary>Identificador único do pátio.</summary>
        public int Id { get; set; }

        /// <summary>Nome do pátio.</summary>
        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>Endereço do pátio.</summary>
        [Required, StringLength(250)]
        public string Endereco { get; set; } = string.Empty;

        /// <summary>Capacidade máxima de motocicletas.</summary>
        [Range(0, 100000)]
        public int Capacidade { get; set; }
    }
}
