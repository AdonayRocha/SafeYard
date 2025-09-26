using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Adicionando serviços ao contêiner
builder.Services.AddControllers();

// Configuração do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<SafeYard.Models.Examples.MotoRequestExample>();

var app = builder.Build();

// Configurando o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
