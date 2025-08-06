using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Nsp.Doc.Ai.Domain.Model;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class DocumentStorage(VectorStore store, IEmbeddingGenerator<string, Embedding<float>> embedding)
    {
        public async Task<int> StoreDocuments(Document[] documents, CancellationToken cancellationToken)
        {
            using var collection = store.GetCollection<ulong, Document>("library");
            await collection.EnsureCollectionExistsAsync(cancellationToken);

            await Task.WhenAll(documents.Select(doc => Task.Run(async () =>
            {
                doc.DefinitionEmbedding = (await embedding.GenerateAsync(doc.Content)).Vector;
            })));

            await collection.UpsertAsync(documents, cancellationToken);

            return documents.Length;
        }
    }
}
