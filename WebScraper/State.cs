using AngleSharp.Html.Dom;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace WebScraper
{
    public static class State
    {
        public static IHtmlDocument CurrentScrapingPageHtml { get; set; }
        public static void ExitAndLog(StackTrace stackTrace, ILogger logger)
        {
            var methodName = stackTrace.GetFrame(0).GetMethod().Name;
            var outerHtml = "";

            logger.LogCritical("Something went wrong in method " + methodName +
                ", OuterHtml will be printed out in the gitlog file.");

            if(CurrentScrapingPageHtml == null)
            {
                logger.LogCritical("Did not scrape any page yet.");
            }
            else
            {
                try
                {
                    outerHtml = CurrentScrapingPageHtml.Children[0].OuterHtml;
                }
                catch
                {
                    logger.LogCritical("Could not get OuterHtml of the page.");
                }
            }

            logger.LogDebug("\n" + outerHtml + "\n\n\n");

            logger.LogInformation("Press any key to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}
