using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using DevExpress.Xpf.Charts;
using PdfManager.Core;
using PdfManager.Core.Services;
using PdfManager.Modules.PdfAnalyzer.Models;
using PdfManager.Modules.PdfAnalyzer.Services;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace PdfManager.Modules.PdfAnalyzer.ViewModels
{
    public class PdfAnalyzerViewModel : BindableBase, INavigationAware
    {
        #region Constructors

        public PdfAnalyzerViewModel(ITextStatisticsService textStatisticsService,
            IPdfReader pdfReader, ITopNWordsHistogram topNWordsHistogram)
        {
            _textStatisticsService = textStatisticsService;
            _pdfReader = pdfReader;
            _topNWordsHistogram = topNWordsHistogram;
            Initialize();
        }

        #endregion Constructors

        #region Fields

        private readonly ITextStatisticsService _textStatisticsService;
        private readonly IPdfReader _pdfReader;
        private readonly ITopNWordsHistogram _topNWordsHistogram;
        private Stream _pdfDocumentStream;
        private bool _isPdfLoaded;
        private int _topNWordsCount;

        private Diagram _topNWords;

        #endregion Fields

        #region Properties

        public Stream PdfDocumentStream
        {
            get { return _pdfDocumentStream; }
            set { SetProperty(ref _pdfDocumentStream, value); }
        }

        public Diagram TopNWords
        {
            get { return _topNWords; }
            set { SetProperty(ref _topNWords, value); }
        }

        public bool IsPdfLoaded
        {
            get { return _isPdfLoaded; }
            set { SetProperty(ref _isPdfLoaded, value); }
        }

        public int TopNWordsCount
        {
            get { return _topNWordsCount; }
            set { SetProperty(ref _topNWordsCount, value); }
        }

        private int PagesCount { get; set; }

        private List<string> UniqueWords { get; set; } = new List<string>();

        private Dictionary<string, int> RepeatedWords { get; set; } = new Dictionary<string, int>();

        private List<string> Sentences { get; set; } = new List<string>();

        private int UniqueWordsCount { get; set; }

        private int SentencesCount { get; set; }

        private int ParagraphsCount { get; set; }

        private int AverageWordLength { get; set; }

        private int AverageSentenceLength { get; set; }

        private string SelectedFilePath { get; set; } 

        #endregion Properties

        #region Commands

        public ICommand OpenFileDialogCommand { get; set; }

        public InteractionRequest<PdfStatisticsNotification> StatisticsInteractionRequest { get; set; }

        public InteractionRequest<INotification> AlertInteractionRequest { get; set; }

        public ICommand StatisticsPopupCommand { get; set; }

        public ICommand TopNWordsCommand { get; set; }

        public DelegateCommand<object> PrintChartCommand { get; set; }

        #endregion Commands

        #region Methods

        public void Initialize()
        {
            // by default show top 5 words
            TopNWordsCount = 5;
            OpenFileDialogCommand = new DelegateCommand(GetPdfFile);
            StatisticsInteractionRequest = new InteractionRequest<PdfStatisticsNotification>();
            AlertInteractionRequest = new InteractionRequest<INotification>();
            StatisticsPopupCommand = new DelegateCommand(ShowStatistics);
            TopNWordsCommand = new DelegateCommand(CreateTopNWordsHistogram);
            PrintChartCommand = new DelegateCommand<object>(PrintChart);
        }

        private void ShowStatistics()
        {
            StatisticsInteractionRequest.Raise(
                new PdfStatisticsNotification
                {
                    Title = "Pdf Statistics",
                    Content = new PdfStatistics
                    {
                        UniqueWordsCount = UniqueWordsCount,
                        AverageWordLength = AverageWordLength,
                        SentencesCount = SentencesCount,
                        AverageSentenceLength = AverageSentenceLength,
                        FilePath = SelectedFilePath
                    }
                });
        }

        private void GetPdfFile()
        {
            SelectedFilePath = Utilities.SelectFileFromDialog(Constants.PdfFilesFilter);
            if (!string.IsNullOrWhiteSpace(SelectedFilePath))
            {
                Clear();
                PdfDocumentStream = Utilities.GetResourceStream(SelectedFilePath);
                // added the next line to fix issue opening more than one file
                Thread.Sleep(1000);
                ReadPdfFileData();
                GetPdfFileStatistics();
                ShowStatistics();
                CreateTopNWordsHistogram();
            }
        }

        private void Clear()
        {
            if (PdfDocumentStream != null)
            {
                PdfDocumentStream.Close();
            }
            UniqueWords = new List<string>();
            RepeatedWords = new Dictionary<string, int>();
            Sentences = new List<string>();
            IsPdfLoaded = false;
        }

        private void ReadPdfFileData()
        {
            _pdfReader.Read(PdfDocumentStream);
            IsPdfLoaded = true;
            PagesCount = _pdfReader.PageCount;
            for (int i = 0; i < PagesCount; i++)
            {
                var pageText = _pdfReader.GetTextOfPage(i);
                var uniqueWordsPerPage = _textStatisticsService.GetUniqueWords(pageText);
                UniqueWords.AddRange(uniqueWordsPerPage);

                var sentencesPerPage = _textStatisticsService.GetSentences(pageText);
                Sentences.AddRange(sentencesPerPage);

                var repeatedWordsPerPage = _textStatisticsService.GetOrderedRepetedWords(pageText);
                FillRepeatedWorsDictionary(repeatedWordsPerPage);
            }
            UniqueWords = UniqueWords.Distinct().ToList();
        }

        private void GetPdfFileStatistics()
        {
            UniqueWordsCount = UniqueWords.Count;
            SentencesCount = Sentences.Count;
            AverageWordLength = _textStatisticsService.GetAverageWordLength(UniqueWords);
            AverageSentenceLength = _textStatisticsService.GetAverageSentenceLength(Sentences);
        }

        private void CreateTopNWordsHistogram()
        {
            IEnumerable<KeyValuePair<string, int>> orderedRepeatedWords =
                            RepeatedWords.OrderByDescending(obj => obj.Value).ToList();
            HistogramOptions options = new HistogramOptions
            {
                SeriesName = $"Top {TopNWordsCount} words",
                BarWidth = 0.5,
                Points = orderedRepeatedWords.Take(TopNWordsCount)
            };
            TopNWords = _topNWordsHistogram.CreateTopNWorsHistogramDiagram(options);
        }

        private void PrintChart(object chart)
        {
            var histogramChart = (ChartControlBase)chart;
            histogramChart.PrintDirect();
        }

        private void FillRepeatedWorsDictionary(Dictionary<string, int> repeatedWords)
        {
            foreach (var item in repeatedWords)
            {
                if (!RepeatedWords.ContainsKey(item.Key))
                {
                    RepeatedWords.Add(item.Key, item.Value);
                }
                else
                {
                    int newCount = RepeatedWords[item.Key] + item.Value;
                    RepeatedWords[item.Key] = newCount;
                }
            }
        }

        #endregion Methods

        #region INavigationAware Members
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // true means navigatable
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
        #endregion INavigationAware Members
    }
}
