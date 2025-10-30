using System.ComponentModel.DataAnnotations.Schema;

namespace SafeYard.Models
{
    [Table("Motos")]
    public class Moto
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Modelo")]
        public required string Modelo { get; set; }

        [Column("Marca")]
        public required string Marca { get; set; }

        [Column("Ano")]
        public int Ano { get; set; }

        [Column("ClienteId")]
        public int? ClienteId { get; set; } 
    }
}
