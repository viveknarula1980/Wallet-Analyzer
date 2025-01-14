using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebScraper.Parsers;

namespace WebScraper.WebScrapers
{
    public class EtherscanDexScraper : IDexScraper
    {
        private readonly IDexTableParser _dexParser;
        private readonly IWebScraper _webScraper;
        private string _url;
        private int _currentPageNumber = 1;

        public EtherscanDexScraper(IDexTableParser parser, IWebScraper webScraper)
        {
            _dexParser = parser;
            _webScraper = webScraper;
        }

        public void Initialize(string url)
        {
            _url = url;
        }

        public async Task<string> ScrapeTokenName(string url)
        {
            var page = await _webScraper.GetPage(url);
            return _dexParser.ParseTokenName(page);
        }
        
        public async Task<List<DexRow>> ScrapeCurrentPageDexRows()
        {
            var currentPageUrl = $"{_url}&p={_currentPageNumber}";
            var page = await _webScraper.GetPage(currentPageUrl);

            if (_dexParser.IsNoMorePages(page))
            {
                return null;
            }

            var table = _dexParser.GetRows(page);

            var allRowTasks = new List<Task<DexRow>>();
            foreach (var row in table.Children)
            {
                var task = _dexParser.ParseRow(row);
                allRowTasks.Add(task);
            }

            var allRows = new List<DexRow>();
            while (allRowTasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(allRowTasks);
                var row = await completedTask;
                allRows.Add(row);
                allRowTasks.Remove(completedTask);
            }

            return allRows;
        }

        public void GoToNextPage()
        {
            _currentPageNumber++;
        }
    }
}
