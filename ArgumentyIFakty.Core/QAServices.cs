using ArgumentyIFakty.Core.Services;
using Majestic13;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core
{
    public static class QAServices
    {
        public static string getQATitle(string htmlText) {
            var parser = new HtmlParser();
            var node = parser.Parse(htmlText);
            var finder = new FindTagsVisitor(tag => tag.Name == "h1" && tag.Attributes.ContainsKey("class")
                && tag.Attributes.ContainsValue("title"));
            node.AcceptVisitor(finder);
          var titleTag = (Majestic13.HtmlNode.Text)finder.Result[0].Children[0];
            var title = titleTag.Value;
            return title;
        }

        public static string getQAAuthor(string htmlText)
        {
            var author = "Автор не известен";
            var parser = new HtmlParser();
            var node = parser.Parse(htmlText);
            var authorFieldFinder = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                && tag.Attributes.ContainsValue("autors_box"));
            node.AcceptVisitor(authorFieldFinder);
            var authorTag = (Majestic13.HtmlNode.Tag)authorFieldFinder.Result[0].Children[1];
            if (authorTag.Attributes.ContainsValue("icon autors_icon"))
            {
                var autorTag = (Majestic13.HtmlNode.Tag)authorFieldFinder.Result[0].Children[2];
                var authorName = (Majestic13.HtmlNode.Text)autorTag.Children[0];
                author = authorName.Value;
            }   
            return author;
        }
        public static DateTime getQADate(string htmlText)
        {
            var parser = new HtmlParser();
            var node = parser.Parse(htmlText);
            var finder = new FindTagsVisitor(tag => tag.Name == "time");
            node.AcceptVisitor(finder);
            var dateTag = (Majestic13.HtmlNode.Text)finder.Result[0].Children[0];
            var datestring = dateTag.Value;
            return DateTime.ParseExact(datestring, "HH:mm dd/MM/yyyy", null);
        
        }
        public static string getQAContent(string htmlText)
        {
            var content = "";
            var parser = new HtmlParser();
            var node = parser.Parse(htmlText);
            var finder = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                && tag.Attributes.ContainsValue("vo_o_text"));
            node.AcceptVisitor(finder);
            var contextTag = finder.Result[0];
            content = RequestServices.ContinueUntilOnlyTextLeft(contextTag, content);
            LogServices.WriteProgressLog("Fetched content of article ");
            return content;
        }
        public static string getQAEditionName(string htmlText)
        {
            var parser = new HtmlParser();
            var node = parser.Parse(htmlText);           
            var finder = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                && tag.Attributes.ContainsValue("autors_box"));
            node.AcceptVisitor(finder);
            var lastIndex = finder.Result[0].Children.Count-2;
            var divTag = (Majestic13.HtmlNode.Tag)finder.Result[0].Children[lastIndex];
            var aTag = (Majestic13.HtmlNode.Tag)divTag.Children[3];
            var name = (Majestic13.HtmlNode.Text)aTag.Children[0];
            return name.Value;
        
        }
    }
}
