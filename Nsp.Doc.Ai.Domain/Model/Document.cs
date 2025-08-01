using Microsoft.Extensions.VectorData;

namespace Nsp.Doc.Ai.Domain.Model
{
    public class Document
    {
        [VectorStoreKey]
        public required ulong Key { get; set; }

        [VectorStoreData]
        public required string Title { get; set; }

        [VectorStoreData]
        public required string Summary { get; set; }

        [VectorStoreVector(1536)]
        public ReadOnlyMemory<float> DefinitionEmbedding { get; set; }
    }
}
