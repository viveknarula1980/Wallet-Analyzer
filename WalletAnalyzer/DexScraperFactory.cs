using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class DexScraperFactory : IDexScraperFactory
    {
        private readonly IDexScraper _dexScraper;

        public DexScraperFactory(IDexScraper dexScraper)
        {
            _dexScraper = dexScraper;
        }

        public IDexScraper CreateScrapper(string url)
        {
            _dexScraper.Initialize(url);
            return _dexScraper;
        }
    }
}
