using System.Collections.Generic;

namespace PdfManager.Core.Services
{
    public interface ITextStatisticsService 
    {
        IEnumerable<string> GetUniqueWords(string text);
        int GetAverageWordLength(IEnumerable<string> words); 
        IEnumerable<string> GetSentences(string text);
        int GetAverageSentenceLength(IEnumerable<string> sentences);
        Dictionary<string, int> GetOrderedRepetedWords(string text); 
        //int GetTotalNumberOfParagraphs(string text);
    }
}
