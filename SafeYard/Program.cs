using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using Swashbuckle.AspNetCore.Filters;
using SafeYard.Services;
using SafeYard;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Controllers
builder.Services.AddControllers();

// Versionamento da API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Health Check
builder.Services.AddHealthChecks();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
    // Adiciona definição de segurança para API Key
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key via header X-API-KEY",
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
    // Registrar o OperationFilter que exibe upload no Swagger
    c.OperationFilter<SafeYard.Swagger.FileUploadOperationFilter>();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<SafeYard.Models.Examples.MotoRequestExample>();

// DI para serviço Roboflow
builder.Services.AddScoped<RoboflowService>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de segurança por API Key
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Endpoint Minimal API para predição via Roboflow (lê o form manualmente para evitar antiforgery binding)
app.MapPost("/api/v1/motos/predict", async (HttpRequest request, RoboflowService roboflowService) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest("Envie uma imagem em multipart/form-data (campo 'image').");

    var form = await request.ReadFormAsync();
    var file = form.Files["image"] ?? form.Files.FirstOrDefault();
    if (file == null)
        return Results.BadRequest("Arquivo não encontrado.");

    using var ms = new System.IO.MemoryStream();
    await file.CopyToAsync(ms);
    var imageBytes = ms.ToArray();

    int motos = await roboflowService.DetectMotos(imageBytes);

    return Results.Ok(new { motosDetectadas = motos });
})
.WithName("DetectMotos")
.Produces(200)
.Produces(400);

app.Run();