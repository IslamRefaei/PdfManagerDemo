using System;
using System.Windows.Input;
using PdfManager.Core;
using PdfManager.Core.Services;
using PdfManager.Modules.PdfAnalyzer.Models;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace PdfManager.Modules.PdfAnalyzer.ViewModels
{
    public class PdfStatisticsViewModel : BindableBase, IInteractionRequestAware
    {
        #region Constructors

        public PdfStatisticsViewModel(IPdfReader pdfReader)
        {
            _pdfReader = pdfReader;
            Initialize();
        }

        #endregion Constructors

        #region Fields

        private PdfStatisticsNotification _pdfStatisticsNotification;
        private IPdfReader _pdfReader;
        private string _paragraphs;  

        #endregion Fields

        #region Properties

        public PdfStatisticsNotification PdfStatisticsNotification
        {
            get { return _pdfStatisticsNotification; }
            set { SetProperty(ref _pdfStatisticsNotification, value); }
        }

        public string Paragraphs 
        {
            get { return _paragraphs; }
            set { SetProperty(ref _paragraphs, value); }
        }

        #endregion Properties

        #region Commands

        public ICommand GetParagraphsCountCommand { get; set; }

        #endregion Commands

        #region Methods

        private void Initialize()
        {
            GetParagraphsCountCommand = new DelegateCommand(GetParagraphsCount);
        }

        private void GetParagraphsCount()
        {
            Paragraphs = "loading ...";
            var filePath = ((PdfStatistics)PdfStatisticsNotification.Content).FilePath;
            byte[] fileBytes = Utilities.GetResourceByteArray(filePath);
            var count = _pdfReader.GetParagraphsCount(fileBytes);
            Paragraphs = count.ToString();
        }

        #endregion Methods

        #region IInteractionRequestAware Members

        public INotification Notification {
            get
            {
               return PdfStatisticsNotification;
            }
            set
            {
                PdfStatisticsNotification = (PdfStatisticsNotification) value;
            }
        }
        public Action FinishInteraction { get; set; }

        #endregion IInteractionRequestAware Members

    }
}
