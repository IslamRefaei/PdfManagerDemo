using System.IO;

namespace PdfManager.Modules.PdfAnalyzer.Models
{
    public class PdfStatistics
    {
        public int UniqueWordsCount { get; set; }

        public int AverageWordLength { get; set; }

        public int SentencesCount { get; set; }

        public int AverageSentenceLength { get; set; } 

        public string FilePath { get; set; }   
    }
}
