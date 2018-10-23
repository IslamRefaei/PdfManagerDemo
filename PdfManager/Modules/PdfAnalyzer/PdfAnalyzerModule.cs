using PdfManager.Core;
using PdfManager.Core.Services;
using PdfManager.Modules.PdfAnalyzer.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PdfManager.Modules.PdfAnalyzer
{
    public class PdfAnalyzerModule : IModule
    {
        protected IRegionManager RegionManager { get; private set; }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            RegionManager = containerProvider.Resolve<IRegionManager>();
            //RegionManager.RequestNavigate(RegionName.MainContentRegion, ViewName.PdfAnalyzerView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(object), typeof(Views.PdfAnalyzer), ViewName.PdfAnalyzerView);
            containerRegistry.Register<ITextStatisticsService, TextStatisticsService>();
            containerRegistry.Register<IPdfReader, PdfReader>();
            containerRegistry.Register<ITopNWordsHistogram, TopNWordsHistogram>();
        }
    }
}
