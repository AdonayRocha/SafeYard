using Swashbuckle.AspNetCore.Filters;
using SafeYard.Models;

namespace SafeYard.Models.Examples
{
    public class ClienteRequestExample : IExamplesProvider<Cliente>
    {
        public Cliente GetExamples()
        {
            return new Cliente
            {
                Nome = "Maria Silva",
                Cpf = "12345678901",
                Email = "maria.silva@email.com"
            };
        }
    }
}