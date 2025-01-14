//Legacy code. Not fully working
//using AngleSharp.Dom;
//using AngleSharp.Html.Dom;
//using System;
//using System.Diagnostics;
//using System.Linq;
//using WebScraper.Parsers;

//namespace WebScraper
//{
//    public class EtherscanTokenDetailsParser : IEtherscanTokenDetailsParser
//    {
//        private readonly IParserCommon _parserCommon;
//        private IElement _tableFromTo;

//        public EtherscanTokenDetailsParser(IParserCommon parserCommon)
//        {
//            _parserCommon = parserCommon;
//        }

//        public void Initialize(IHtmlDocument page)
//        {
//            _tableFromTo = StepToTableFromTo(page);
//        }

//        public string ParseBuyer()
//        {
//            if (_tableFromTo == null)
//            {
//                return "";
//                Console.WriteLine("Txn has not found");
//            }

//            var current = _tableFromTo;

//            _parserCommon.StepIfMatches(ref current, current.Id, "ContentPlaceHolder1_maintable", current);


//            var temp = current.Children.Where(x => x.ClassName == "row align-items-center mb-4");
//            temp = temp.Where(x => x.Children[0].ClassName == "col-md-3 font-weight-bold font-weight-md-normal mb-1 mb-md-0").ToList();
//            current = temp.FirstOrDefault(x => x.Children[0].Children[0].TagName == "I"
//            && x.Children[0].Children[0].OuterHtml.Contains("The sending party of the transaction."));

//            if (current is null)
//            {
//                State.ExitAndLog(new StackTrace());
//            }

//            current = current.Children[1];
//            _parserCommon.StepIfMatches(ref current, current.ClassName, "col-md-9", current.Children[0]);

//            var result = _parserCommon.GetDataIfMatches(current.TextContent, current.Id, "spanFromAdd");

//            return result;

//        }

//        public string ParseSeller()
//        {
//            if (_tableFromTo == null)
//            {
//                return "";
//                Console.WriteLine("Txn has not found");
//            }

//            var current = _tableFromTo;
//            var result = "";

//            _parserCommon.StepIfMatches(ref current, current.Id, "ContentPlaceHolder1_maintable", current);

//            current = current.Children.Where(x => x.ClassName == "row mb-4")
//            .Where(x => x.Children[0].ClassName == "col-md-3 font-weight-bold font-weight-md-normal mb-1 mb-md-0")
//            .FirstOrDefault(x =>
//                x.Children[0].Children[0].TagName == "I"
//                && x.Children[0].Children[0].OuterHtml.Contains("The receiving party of the transaction (could be a contract address).")
//                );

//            if (current is null)
//            {
//                State.ExitAndLog(new StackTrace());
//            }
//            current = current.Children[1];
//            _parserCommon.StepIfMatches(ref current, current.ClassName, "col-md-9", current);
//            result = current.Children.FirstOrDefault(x => x.Id == "contractCopy" && x.ClassName == "wordwrap mr-1").TextContent;
            
//            return result;
//        }

//        private IElement StepToTableFromTo(IHtmlDocument page)
//        {
//            var current = page.Body.Children[0];
//            _parserCommon.StepIfMatches(ref current, current.ClassName, "wrapper", current.Children[1]);
//            _parserCommon.StepIfMatches(ref current, current.Id, "content", current.Children[10]);
//            _parserCommon.StepIfMatches(ref current, current.ClassName, "container space-bottom-2", current.Children[0]);
//            _parserCommon.StepIfMatches(ref current, current.ClassName, "card", current.Children[1]);
//            _parserCommon.StepIfMatches(ref current, current.Id, "myTabContent", current.Children[0]);
//            _parserCommon.StepIfMatches(ref current, current.Id, "home", current.Children[0]);
//            _parserCommon.StepIfMatches(ref current, current.Id, "ContentPlaceHolder1_maintable", current);

//            return current;
//        }
//    }
//}
