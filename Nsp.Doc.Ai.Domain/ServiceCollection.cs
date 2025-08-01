using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Nsp.Doc.Ai.Domain.Services;
using Qdrant.Client;

namespace Nsp.Doc.Ai.Domain
{
    public static class ServiceCollection
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQdrantVectorStore((sp) => new QdrantClient(
                host: configuration["QdrantHost"]!,
                https: true,
                apiKey: configuration["QdrantKey"]!)
            );

            services.AddAzureOpenAIEmbeddingGenerator(deploymentName: "text-embedding-3-large",
                endpoint: configuration["AzureOpenAiEndpoint"]!,
                apiKey: configuration["AzureOpenAiKey"]!,
                modelId: "text-embedding-3-large",
                dimensions: 1536
            );

            services.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt-4o",
                apiKey: configuration["AzureOpenAiKey"]!,
                endpoint: configuration["AzureOpenAiEndpoint"]!,
                modelId: "gpt-4o"
            );

            services.AddScoped<DocumentReader>();
            services.AddScoped<DocumentStorage>();
            services.AddScoped<ChatService>();

            services.AddSingleton<DocumentSearchPlugin>();
            
            services.AddSingleton<KernelPluginCollection>((serviceProvider) => 
                [
                    KernelPluginFactory.CreateFromObject(serviceProvider.GetRequiredService<DocumentSearchPlugin>())
                ]
            );

            services.AddTransient((serviceProvider)=> {
                var pluginCollection = serviceProvider.GetRequiredService<KernelPluginCollection>();

                return new Kernel(serviceProvider, pluginCollection);
            });
        }
    }
}
