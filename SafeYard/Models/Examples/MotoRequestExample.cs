using Swashbuckle.AspNetCore.Filters;
using SafeYard.Models;

namespace SafeYard.Models.Examples
{
    public class MotoRequestExample : IExamplesProvider<Moto>
    {
        public Moto GetExamples()
        {
            return new Moto
            {
                Modelo = "CB 500F",
                Marca = "Honda",
                Ano = 2022
            };
        }
    }
}