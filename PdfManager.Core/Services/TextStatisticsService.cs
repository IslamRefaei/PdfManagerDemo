using System;
using System.Collections.Generic;
using System.Linq;

namespace PdfManager.Core.Services
{
    public class TextStatisticsService : ITextStatisticsService
    {
        public int GetAverageSentenceLength(IEnumerable<string> sentences)
        {
            int length = 0;
            if (sentences.Any())
            {
                var averageSentenceLength = sentences.Average(n => n.Length);
                length = Approximate(averageSentenceLength);
            }
            return length;
        }

        public int GetAverageWordLength(IEnumerable<string> words)
        {
            int length = 0;
            if (words.Any())
            {
                var averageWordLength = words.Average(n => n.Length);
                length =  Approximate(averageWordLength);
            }
            return length;
        }

        public Dictionary<string, int> GetOrderedRepetedWords(string text)
        {
            var words = GetWords(text);
            Dictionary<string, int> repeatedWords = words.GroupBy(x => x)
                         .Where(group => group.Count() > 1).OrderByDescending(group => group.Count())
                         .ToDictionary(gdc => gdc.Key, gdc => gdc.Count());
            return repeatedWords;
        }

        public IEnumerable<string> GetSentences(string text)
        {
            IEnumerable<string> sentences = new List<string>();
            if (!string.IsNullOrWhiteSpace(text))
            {
                sentences = text.ToLower().Split('.')?.Where(w => !string.IsNullOrWhiteSpace(w));
            }
            return sentences;
        }

        public IEnumerable<string> GetUniqueWords(string text)
        {
            var words = GetWords(text);
            IEnumerable<string> uniquedWords = words.Distinct().ToArray();
            return uniquedWords;
        }

        private int Approximate(double number)
        {
            var num = Math.Round(number, 0, MidpointRounding.AwayFromZero);
            return Convert.ToInt32(num);
        }

        private IEnumerable<string> GetWords(string text)
        {
            IEnumerable<string> words = new List<string>();
            if (!string.IsNullOrWhiteSpace(text))
            {
                words = text.ToLower().Split().Where(w => w.Length >= Constants.MinimumWordLength);
            }
            return words;
        }
    }
}
