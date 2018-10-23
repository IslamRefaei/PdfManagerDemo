using System.IO;

namespace PdfManager.Core.Services
{
    public interface IPdfReader
    {
        void Read(Stream fileStream);

        void Read(byte[] bytes);

        int PageCount { get; }

        string GetTextOfPage(int pageNumber);

        int GetParagraphsCount(byte[] bytes); 
    }
}
