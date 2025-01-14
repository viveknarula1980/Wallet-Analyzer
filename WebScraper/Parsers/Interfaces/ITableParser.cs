using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Threading.Tasks;
using WebScraper.WebScrapers;

namespace WebScraper.Parsers
{
    public interface IDexTableParser
    {
        IElement GetRows(IHtmlDocument page);
        Task<DexRow> ParseRow(IElement rowHtml); //fix maybe Parse method shouldnt be async
        string ParseTokenName(IHtmlDocument page);
        bool IsNoMorePages(IHtmlDocument page);
    }
}