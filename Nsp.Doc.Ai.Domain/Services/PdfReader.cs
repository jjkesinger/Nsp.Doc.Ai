using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class PdfReader
    {
        public string ReadPdfContent(byte[] pdfContents)
        {
            var sb = new StringBuilder();
            using PdfDocument document = PdfDocument.Open(pdfContents);
            foreach (Page page in document.GetPages())
            {
                string text = ContentOrderTextExtractor.GetText(page);
                sb.Append(text);
            }

            return sb.ToString();
        }
    }
}