using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PdfManager.Core.Services;

namespace PdfManager.Core.Tests.Services
{
    [TestFixture]
    public class TextStatisticsServiceTest : UnitTestBase
    {
        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random _random = new Random();
        public ITextStatisticsService ServiceUnderTest => AutoMock.Create<TextStatisticsService>();

        [Test]
        public void GetUniqueWords_ShouldReturnValidUniqueWords()
        {
            //Arrange
            const int ExpectedWordsCount = 14;
            var text = GetText();

            //Act
            var result = ServiceUnderTest.GetUniqueWords(text);

            //Assert
            result.Count()
                .Should()
                .Be(ExpectedWordsCount);
        }

        [Test] 
        public void GetSentences_ShouldReturnCorrectNumber() 
        {
            //Arrange
            const int ExpectedSentencesCount = 4; 
            var text = GetText();

            //Act
            var result = ServiceUnderTest.GetSentences(text);

            //Assert
            result.Count()
                .Should()
                .Be(ExpectedSentencesCount);
        }

        [Test]
        public void GetAverageWordLength_ShouldReturnFive_WithThreeWords_WithTotalFifteenCharacters()
        {
            //Arrange
            var sentences = new List<string>
            {
                CreateWord(4),
                CreateWord(6),
                CreateWord(5)
            };

            //Act
            var result = ServiceUnderTest.GetAverageWordLength(sentences);

            //Assert
            result
                .Should()
                .Be(5);
        }

        [Test]
        public void GetAverageSentenceLength_ShouldReturnTen_WithThreeSentences_WithTotalThirtynCharacters() 
        {
            //Arrange
            var sentences = new List<string>
            {
                CreateSentence(2, 9),
                CreateSentence(3, 14),
                CreateSentence(2, 7)
             };

            //Act
            var result = ServiceUnderTest.GetAverageSentenceLength(sentences);

            //Assert
            result
                .Should()
                .Be(10);
        }

        private string CreateSentence(int wordCount, int sentenceLength)
        {
            const int WordLength = 3;
            var sentence = string.Empty;
            for (int i = 0; i < wordCount; i++)
            {
                sentence = (i == wordCount - 1) ? sentence += CreateWord(sentenceLength - ((i * WordLength) + i))
                                                : sentence += CreateWord(WordLength) + " ";
            }
            return sentence;
        }

        private string CreateWord(int length)
        {
            var word = new string(Enumerable.Repeat(Chars, length)
                                            .Select(s => s[_random.Next(s.Length)]).ToArray());
            return word;
        }

        private string GetText()
        {
            string text =  @"my name is islam refaei.
                                i'm software engineer.
                                this is wpf task.
                                thanks for this chance";
            return text;
        }
    }
}
