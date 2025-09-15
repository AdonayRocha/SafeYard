using Swashbuckle.AspNetCore.Filters;
using SafeYard.Models;

namespace SafeYard.Models.Examples
{
    public class PatioRequestExample : IExamplesProvider<Patio>
    {
        public Patio GetExamples()
        {
            return new Patio
            {
                Nome = "Pátio Central",
                Endereco = "Rua das Flores, 123",
                Capacidade = 100
            };
        }
    }
}