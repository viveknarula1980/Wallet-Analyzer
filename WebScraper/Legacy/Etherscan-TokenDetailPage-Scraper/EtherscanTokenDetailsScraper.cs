//Legacy code
//using System.Threading.Tasks;
//using WebScraper.WebScrapers;

//namespace WebScraper
//{
//    public class EtherscanTokenDetailsScraper : IEtherscanTokenDetailsScraper
//    {
//        private readonly IWebScraper _webScraper;
//        private readonly IEtherscanTokenDetailsParserFactory _parserFactory;
//        private IEtherscanTokenDetailsParser _parser;

//        public EtherscanTokenDetailsScraper(IWebScraper webScraper, IEtherscanTokenDetailsParserFactory parserFactory)
//        {
//            _webScraper = webScraper;
//            _parserFactory = parserFactory;
//        }

//        public async Task InitializeAsync(string url)
//        {
//            var page = await _webScraper.GetPage(url);
//            _parser = _parserFactory.CreateParser(page);
//        }

//        public string ScrapeBuyerHash(string txnHash)
//        {
//            return _parser.ParseBuyer();
//        }

//        public string ScrapeSellerHash(string txnHash)
//        {
//            return _parser.ParseSeller();
//        }
//    }
//}