using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;
using Nsp.Doc.Ai.Domain.Model;
using System.ClientModel;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class DocumentStorage(VectorStore store, IConfiguration configuration)
    {
        public async Task StoreDocuments(string collectionName, Document[] documents, CancellationToken cancellationToken)
        {
            var collection = store.GetCollection<ulong, Document>(collectionName);
            await collection.EnsureCollectionExistsAsync(cancellationToken);

            await GenerateEmbedding(documents);

            await collection.UpsertAsync(documents, cancellationToken);
        }

        private async Task GenerateEmbedding(Document[] documents)
        {
            var embedding = new AzureOpenAIClient(new Uri(configuration["AzureOpenAiEndpoint"]!),
                new ApiKeyCredential(configuration["AzureOpenAiKey"]!))
                    .GetEmbeddingClient("document_embed")
                    .AsIEmbeddingGenerator(1536);

            var tasks = documents.Select(entry => Task.Run(async () =>
            {
                entry.DefinitionEmbedding = (await embedding.GenerateAsync(entry.Summary)).Vector;
            }));

            await Task.WhenAll(tasks);
        }
    }
}
