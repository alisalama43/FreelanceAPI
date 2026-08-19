using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace FreelanceAPI.OpenApi.Transformers;

public sealed class VersionInfoTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Freelance Marketplace API",
            Version = context.DocumentName,
            Description = "REST API for managing users, services, orders, reviews, authentication and authorization."
        };

        return Task.CompletedTask;
    }
}