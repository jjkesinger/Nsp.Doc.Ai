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
                if (file.FileType == "text/plain" || file.FileType == "text/html")
                {
                    var contents = System.Text.Encoding.UTF8.GetString(file.Contents);
                    if (!string.IsNullOrWhiteSpace(contents))
                    {
                        docs.Add(new Document
                        {
                            Key = Guid.NewGuid(),
                            Title = file.FileName,
                            Content = contents
                        });
                    }
                }
                else if (file.FileType == "application/pdf")
                {
                    var contents = pdfReader.ReadPdfContent(file.Contents, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(contents))
                    {
                        docs.Add(new Document
                        {
                            Key = Guid.NewGuid(),
                            Title = file.FileName,
                            Content = contents
                        });
                    }
                }
            });

            return await Task.FromResult<Document[]>([.. docs]);
        }
    }
}
