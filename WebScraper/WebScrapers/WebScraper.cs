using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.WebScrapers;

namespace WebScraper
{
    public class WebScraper : IWebScraper
    {
        public async Task<IHtmlDocument> GetPage(string url)
        {
            var cancellationToken = new CancellationTokenSource();
            var httpClient = new HttpClient();

            var request = await httpClient.GetAsync(url);
            cancellationToken.Token.ThrowIfCancellationRequested();

            var response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            var parser = new HtmlParser();

            cancellationToken.Dispose();
            httpClient.Dispose();

            var page = parser.ParseDocument(response);
            State.CurrentScrapingPageHtml = page;

            return page;
        }
    }
}
