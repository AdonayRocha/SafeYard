using System.ComponentModel.DataAnnotations;

namespace SafeYard.Models
{
    /// <summary>
    /// Representa um cliente/proprietário de motocicleta.
    /// </summary>
    public class Cliente
    {
        /// <summary>Identificador único do cliente.</summary>
        public int Id { get; set; }

        /// <summary>Nome completo do cliente.</summary>
        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>CPF do cliente (apenas dígitos).</summary>
        [Required, RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos.")]
        public string Cpf { get; set; } = string.Empty;

        /// <summary>Email de contato do cliente.</summary>
        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;
    }
}
