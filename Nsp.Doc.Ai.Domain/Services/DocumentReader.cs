using System.Collections.Concurrent;
using Nsp.Doc.Ai.Domain.Model;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class DocumentReader(PdfReader pdfReader)
    {
        public async Task<Document[]> ReadDocuments(List<(string FileName, string FileType, byte[] Contents)> files, CancellationToken cancellationToken)
        {
            var docs = new ConcurrentBag<Document>();
            Parallel.ForEach(files, (file) =>
            {
                if (file.FileType == "text/plain")
                {
                    docs.Add(new Document
                    {
                        Key = Guid.NewGuid(),
                        Title = file.FileName,
                        Content = System.Text.Encoding.UTF8.GetString(file.Contents)
                    });
                }
                else if (file.FileType == "application/pdf")
                {
                    docs.Add(new Document
                    {
                        Key = Guid.NewGuid(),
                        Title = file.FileName,
                        Content = pdfReader.ReadPdfContent(file.Contents)
                    });
                }
            });

            return await Task.FromResult<Document[]>([.. docs]);
        }
    }
}
