using AngleSharp.Dom;

namespace WebScraper.Parsers
{
    public interface IParserCommon
    {
        void StepIfMatches(ref IElement current, string actualAttribute, string expectedAttribute, IElement nextStep);
        string GetDataIfMatches(string result, string actualAttribute, string expectedAttribute);
    }
}