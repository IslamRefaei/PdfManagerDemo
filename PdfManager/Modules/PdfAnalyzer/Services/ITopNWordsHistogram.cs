using DevExpress.Xpf.Charts;
using PdfManager.Modules.PdfAnalyzer.Models;

namespace PdfManager.Modules.PdfAnalyzer.Services
{
    public interface ITopNWordsHistogram
    {
        Diagram CreateTopNWorsHistogramDiagram(HistogramOptions options);
    }
}
