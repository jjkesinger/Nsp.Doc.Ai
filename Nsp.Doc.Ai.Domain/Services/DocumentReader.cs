using Nsp.Doc.Ai.Domain.Model;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class DocumentReader
    {
        public async Task<Document[]> ReadDocuments(List<(string FileName, string FileType, byte[] Contents)> files, CancellationToken cancellationToken)
        {
            var docs = new List<Document>();
            foreach(var (FileName, _, Contents) in files)
            {
                docs.Add(new Document
                {
                    Key = Guid.NewGuid(),
                    Title = FileName,
                    Content = Contents.Length > 0 ? System.Text.Encoding.UTF8.GetString(Contents) : string.Empty,
                });
            }

            return await Task.FromResult(docs.ToArray());
        }
    }
}
