using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafeYard.Swagger
{
    public class HealthCheckDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!swaggerDoc.Paths.ContainsKey("/health"))
            {
                swaggerDoc.Paths.Add("/health", new OpenApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Get] = new OpenApiOperation
                        {
                            Summary = "Health Check",
                            Description = "Verifica se a API está saudável.",
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new OpenApiResponse { Description = "API saudável" }
                            }
                        }
                    }
                });
            }
        }
    }
}