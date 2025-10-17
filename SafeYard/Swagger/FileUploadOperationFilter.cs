using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafeYard.Swagger
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
          
            var isPredictEndpoint = context.ApiDescription.RelativePath?.Equals("api/v1/motos/predict", StringComparison.OrdinalIgnoreCase) ?? false;

            if (!isPredictEndpoint)
                return;

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                ["image"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary",
                                    Description = "Arquivo de imagem (campo 'image')"
                                }
                            },
                            Required = new HashSet<string> { "image" }
                        }
                    }
                }
            };
        }
    }
}