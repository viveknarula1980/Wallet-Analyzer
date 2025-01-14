using System.Collections.Generic;

namespace WebScraper.WebScrapers
{
    public class DexTable
    {
        public string TokenName { get; set; }
        public List<DexRow> Rows { get; set; } = new List<DexRow>();
    }
}
