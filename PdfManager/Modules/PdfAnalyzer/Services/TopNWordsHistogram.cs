using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Charts;
using PdfManager.Modules.PdfAnalyzer.Models;

namespace PdfManager.Modules.PdfAnalyzer.Services
{
    public class TopNWordsHistogram : ITopNWordsHistogram
    {
        Diagram TopNWords { get; set; }
        public Diagram CreateTopNWorsHistogramDiagram(HistogramOptions options)
        {
            TopNWords = new XYDiagram2D();
            Add2DBarSeries(options.SeriesName, options.BarWidth);
            AddPointsToSeries(options.SeriesName, options.Points);
            return TopNWords;
        }

        private void Add2DBarSeries(string seriesName, double barWidth) 
        {
            if(TopNWords != null && !string.IsNullOrWhiteSpace(seriesName))
            {
                TopNWords.Series.Add(new BarSideBySideSeries2D
                {
                    DisplayName = seriesName,
                    BarWidth = barWidth,
                });
            }
        }

        private void AddPointsToSeries(string seriesName, IEnumerable<KeyValuePair<string, int>> points)
        {
            if(TopNWords != null && !string.IsNullOrWhiteSpace(seriesName) && points.Any())
            {
                var currentSeries = TopNWords.Series.FirstOrDefault(s => s.DisplayName == seriesName);
                if(currentSeries != null)
                {
                    foreach (var point in points)
                    {
                        currentSeries.Points.Add(CreateSeriesPoint(point));
                    }
                }
            }
        }

        private SeriesPoint CreateSeriesPoint(KeyValuePair<string,int> point)
        {
            var seriesPoint = new SeriesPoint
            {
                Argument = point.Key,
                Value = point.Value
            };
            return seriesPoint;
        }
    }
}
