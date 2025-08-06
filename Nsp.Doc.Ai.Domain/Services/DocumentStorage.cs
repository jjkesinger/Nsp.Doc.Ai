using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Nsp.Doc.Ai.Domain.Model;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class DocumentStorage(VectorStore store, IEmbeddingGenerator<string, Embedding<float>> embedding)
    {
        public async Task StoreDocuments(Document[] documents, CancellationToken cancellationToken)
        {
            using var collection = store.GetCollection<ulong, Document>("library");
            await collection.EnsureCollectionExistsAsync(cancellationToken);

            await GenerateEmbedding(documents);

            await collection.UpsertAsync(documents, cancellationToken);
        }

        private async Task GenerateEmbedding(Document[] documents)
        {
            var tasks = documents.Select(entry => Task.Run(async () =>
            {
                entry.DefinitionEmbedding = (await embedding.GenerateAsync(entry.Content)).Vector;
            }));

            await Task.WhenAll(tasks);
        }
    }
}
