using System.Collections.Generic;

namespace PdfManager.Modules.PdfAnalyzer.Models
{
    public class HistogramOptions
    {
        public string SeriesName { get; set; }

        public double BarWidth { get; set; } 

        public IEnumerable<KeyValuePair<string, int>> Points { get; set; }  
    }
}
