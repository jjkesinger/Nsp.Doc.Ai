using System.ComponentModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Nsp.Doc.Ai.Domain.Model;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class DocumentSearchPlugin(VectorStore store, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        [KernelFunction("SearchTheDocuments")]
        [Description("Search for a document similar to the given query.")]
        public async Task<string[]> SearchAsync(string collectionName, string query, CancellationToken cancellationToken)
        {
            var embedding = await embeddingGenerator.GenerateVectorAsync(query, cancellationToken: cancellationToken);
            var collection = store.GetCollection<Guid, Document>(collectionName);
            var response = collection.SearchAsync(embedding, 20, new VectorSearchOptions<Document>() { VectorProperty = d => d.DefinitionEmbedding, }, CancellationToken.None);

            var results = response.ToBlockingEnumerable(cancellationToken);

            return [.. results.Select(r => r.Record.Summary)];
        }
    }
}