using AngleSharp.Html.Dom;
using System.Threading.Tasks;

namespace WebScraper.WebScrapers
{
    public interface IWebScraper
    {
        public Task<IHtmlDocument> GetPage(string url);
    }
}