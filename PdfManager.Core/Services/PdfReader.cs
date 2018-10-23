using System.IO;
using System.Linq;
using IronPdf;
using SautinSoft.Document;

namespace PdfManager.Core.Services
{
    public class PdfReader : IPdfReader
    {
        PdfDocument _pdfDocument;

        int IPdfReader.PageCount { get => _pdfDocument != null ? _pdfDocument.PageCount : 0; }

        public int GetParagraphsCount(byte[] bytes)  
        {
            var paragraphsCount = 0;
            DocumentCore pdfDocument = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                pdfDocument = DocumentCore.Load(ms, new PdfLoadOptions());
            }
            if (pdfDocument != null)
            {
                var blocks = pdfDocument.Sections.SelectMany(s => s.Blocks);
                blocks = blocks.Where(b => !string.IsNullOrWhiteSpace(b.Content.ToString()));
                paragraphsCount = blocks.Count();
            }
            return paragraphsCount;
        }

        public string GetTextOfPage(int pageNumber)
        {
            string pageText = string.Empty;
            if (_pdfDocument != null)
            {
                pageText = _pdfDocument.ExtractTextFromPage(pageNumber);
            }
            return pageText;
        }

        public void Read(Stream fileStream)
        {
            _pdfDocument = new PdfDocument(fileStream);
        }

        public void Read(byte[] bytes)
        {
            _pdfDocument = new PdfDocument(bytes);
        }


    }
}
