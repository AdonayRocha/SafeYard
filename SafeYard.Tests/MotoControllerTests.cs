using Xunit;
using SafeYard.Controllers;
using SafeYard.Models;
using Microsoft.AspNetCore.Mvc;
using SafeYard.Data;
using Microsoft.EntityFrameworkCore;

public class MotoControllerTests
{
    [Fact]
    public async Task PostMoto_ModeloVazio_RetornaBadRequest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MotoDb").Options;
        var context = new ApplicationDbContext(options);
        var controller = new MotoController(context);

        var moto = new Moto { Modelo = "", Marca = "Honda", Ano = 2020, ClienteId = 1 };
        var result = await controller.PostMoto(moto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}