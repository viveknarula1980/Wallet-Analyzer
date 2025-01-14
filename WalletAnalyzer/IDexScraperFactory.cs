using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public interface IDexScraperFactory
    {
        IDexScraper CreateScrapper(string url);
    }
}