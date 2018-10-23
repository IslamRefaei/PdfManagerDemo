using Prism.Interactivity.InteractionRequest;

namespace PdfManager.Modules.PdfAnalyzer.Models
{
    public class PdfStatisticsNotification: INotification
    {
        public string Title { get; set; }

        public object Content { get; set; }
    }
}
