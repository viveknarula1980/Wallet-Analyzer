using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using WebScraper.Parsers;

namespace WebScraper
{
    public class ParserCommon : IParserCommon
    {
        public void StepIfMatches(ref IElement current, string actualAttribute, string expectedAttribute, IElement nextStep)
        {
            if (expectedAttribute != actualAttribute)
            {
                throw new InvalidOperationException($"attribute {actualAttribute} was expected to be equal to {expectedAttribute}");
            }
            current = nextStep;
        }

        public string GetDataIfMatches(string result, string actualAttribute, string expectedAttribute)
        {
            if (expectedAttribute != actualAttribute)
            {
                throw new InvalidOperationException($"attribute {actualAttribute} was expected to be equal to {expectedAttribute}");
            }
            return result;
        }
    }
}
