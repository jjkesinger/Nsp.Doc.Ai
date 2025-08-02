using System.ComponentModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Nsp.Doc.Ai.Domain.Model;

namespace Nsp.Doc.Ai.Domain.Plugins
{
    public class DocumentSearchPlugin(VectorStore store, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        [KernelFunction("SearchTheDocuments")]
        [Description("Search for a document in the library similar to the given query.")]
        public async Task<Document[]> SearchAsync(string query, CancellationToken cancellationToken)
        {
            var collection = store.GetCollection<Guid, Document>("library");

            var results = collection.SearchAsync(
                    searchValue: await embeddingGenerator.GenerateVectorAsync(query, cancellationToken: cancellationToken),
                    top: 10,
                    cancellationToken: cancellationToken)
                .ToBlockingEnumerable(cancellationToken);
            
            return [.. results.Select(r => r.Record)];
        }
    }
}