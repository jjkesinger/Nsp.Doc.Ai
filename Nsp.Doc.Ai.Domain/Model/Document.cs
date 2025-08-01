using System.Text.Json.Serialization;
using Microsoft.Extensions.VectorData;

namespace Nsp.Doc.Ai.Domain.Model
{
    public class Document
    {
        [VectorStoreKey]
        [JsonPropertyName("key")]
        public required Guid Key { get; set; }

        [VectorStoreData]
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [VectorStoreData]
        [JsonPropertyName("content")]
        public required string Content { get; set; }

        [VectorStoreVector(1536)]
        [JsonPropertyName("embedding")]
        public ReadOnlyMemory<float> DefinitionEmbedding { get; set; }
    }
}
