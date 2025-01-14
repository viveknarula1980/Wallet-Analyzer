using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebScraper.WebScrapers
{
    public interface IDexScraper
    {
        Task<List<DexRow>> ScrapeCurrentPageDexRows();
        void GoToNextPage();
        void Initialize(string url);
        Task<string> ScrapeTokenName(string url);
    }
}