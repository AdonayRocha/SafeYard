using Xunit;
using SafeYard.Controllers;
using Microsoft.AspNetCore.Mvc;
using SafeYard.Data;
using Microsoft.EntityFrameworkCore;

public class ClientesControllerTests
{
    [Fact]
    public async Task DeleteCliente_ClienteNaoExiste_RetornaNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ClienteDb").Options;
        var context = new ApplicationDbContext(options);
        var controller = new ClientesController(context);

        var result = await controller.DeleteCliente(999);

        Assert.IsType<NotFoundResult>(result);
    }
}