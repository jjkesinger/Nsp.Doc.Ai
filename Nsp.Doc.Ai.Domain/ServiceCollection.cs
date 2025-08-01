using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Nsp.Doc.Ai.Domain.Services;
using Qdrant.Client;

namespace Nsp.Doc.Ai.Domain
{
    public static class ServiceCollection
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DocumentReader>();
            services.AddScoped<DocumentStorage>();

            services.AddScoped<VectorStore>((sp) =>
            {
                var client = new QdrantClient(
                            host: configuration["QdrantHost"]!,
                            https: true,
                            apiKey: configuration["QdrantKey"]!
                        );

                var store = new QdrantVectorStore(client, true);

                return store;
            });
        }
    }
}
