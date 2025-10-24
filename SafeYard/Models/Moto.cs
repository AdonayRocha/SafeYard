namespace SafeYard.Models
{
    public class Moto
    {
        public int Id { get; set; }
        public required string Modelo { get; set; }
        public required string Marca { get; set; }
        public int Ano { get; set; }
        public int ClienteId { get; set; } 
    }
}
