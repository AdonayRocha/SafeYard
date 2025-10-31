using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeYard.Models
{
    /// <summary>
    /// Representa uma motocicleta cadastrada no sistema.
    /// </summary>
    [Table("Motos")]
    public class Moto
    {
        /// <summary>
        /// Identificador único da moto.
        /// </summary>
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Modelo da moto (ex.: CG 160, CB 500F).
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [Column("Modelo")]
        public required string Modelo { get; set; }

        /// <summary>
        /// Marca da moto (ex.: Honda, Yamaha).
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [Column("Marca")]
        public required string Marca { get; set; }

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        [Range(1900, 9999)]
        [Column("Ano")]
        public int Ano { get; set; }

        /// <summary>
        /// Identificador do cliente proprietário (opcional).
        /// </summary>
        [Column("ClienteId")]
        public int? ClienteId { get; set; }
    }
}
