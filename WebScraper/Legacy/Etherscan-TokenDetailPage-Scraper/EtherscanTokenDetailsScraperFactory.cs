//Legacy code
//using System.Threading.Tasks;
//using WebScraper.Parsers;

//namespace WebScraper
//{
//    public class EtherscanTokenDetailsScraperFactory : IEtherscanTokenDetailsScraperFactory
//    {
//        private readonly IEtherscanTokenDetailsScraper _scraper;

//        public EtherscanTokenDetailsScraperFactory(IEtherscanTokenDetailsScraper scraper)
//        {
//            _scraper = scraper;
//        }

//        public async Task<IEtherscanTokenDetailsScraper> CreateScraperAsync(string url)
//        {
//            await _scraper.InitializeAsync(url);
//            return _scraper;
//        }
//    }
//}
