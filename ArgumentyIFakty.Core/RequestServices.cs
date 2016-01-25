
using ArgumentyIFakty.Core.Services;
using ArgumentyIFakty.DataAccess;
using Majestic13;
using Supremes.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core
{
    public static class RequestServices
    {
        static IArticleService _articleService;


        static IArticleService articleService
        {
            get
            {
                if (_articleService == null)
                    _articleService = new ArticleEFService();
                return _articleService;
            }
        }
        public static string GetWebText(string url)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                
                response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream());
                string htmlText = sr.ReadToEnd();           
                LogServices.WriteProgressLog("htmlText fetched " + url);
                return htmlText;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                    LogServices.WriteProgressLog("Page not found. Errorcode:" + ((int)response.StatusCode).ToString());
                    return null;
                    
                }
                else
                {
                    LogServices.WriteProgressLog("Error:" + (e.Status).ToString());
                    return null;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();                    
                }
            }       
           
        }

        public static bool isWeeklyNewspaper(string htmlText) {
            try
            {
                // if containts the word ejenedelnik we need this
                var parser = new HtmlParser();
                var node = parser.Parse(htmlText);
                var finder = new FindTagsVisitor(tag => tag.Name == "h1" && tag.Attributes.ContainsKey("class") && tag.Attributes.ContainsValue("title"));
                node.AcceptVisitor(finder);
                var titleNode = (Majestic13.HtmlNode.Text)finder.Result[0].Children[0];
                var title = titleNode.Value;
                Regex regEx = new Regex(@"(Еженедельник \""Аргументы и Факты\"" №)");
                Match m = regEx.Match(title);

                if (m.Success)
                {
                    LogServices.WriteProgressLog("it is weekly ");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                LogServices.WriteProgressLog("исключение");
                return false;
            }   
        }

        public static List<string> extractLinks(string htmlNewspaperText)
        {
            try
            {
                var links = new List<string>();
                var parser = new HtmlParser();
                var node = parser.Parse(htmlNewspaperText);
                var areaFinder = new FindTagsVisitor(tag => tag.Name == "h2" && tag.Attributes.ContainsKey("class") && tag.Attributes.ContainsValue("data_title mbottom10"));
                node.AcceptVisitor(areaFinder);
                var linksarea = areaFinder.Result;
                foreach (Majestic13.HtmlNode.Tag tag in linksarea)
                {
                    var linkFinder = new FindTagsVisitor(t => t.Name == "a" && t.Attributes.ContainsKey("href"));
                    tag.AcceptVisitor(linkFinder);
                    var link = linkFinder.Result[0].Attributes.Values.FirstOrDefault();
                    links.Add(link.ToString());
                }
                LogServices.WriteProgressLog("extracted links");
                return links;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static string getArticleTitle(string htmlText) {
            try
            {
                var title = "Без заголовка";
                var parser = new HtmlParser();
                var node = parser.Parse(htmlText);
                var titleFinder = new FindTagsVisitor(tag => tag.Name == "h1" && tag.Attributes.ContainsKey("class")
                    && tag.Attributes.ContainsValue("material_title increase_text"));
                node.AcceptVisitor(titleFinder);
                if (titleFinder.Result.Count > 0 && titleFinder.Result[0].Children.Count > 0)
                {
                    var titleTag = (Majestic13.HtmlNode.Text)titleFinder.Result[0].Children[0];
                    title = titleTag.Value;
                }
                return title;

            }
            catch (Exception)
            {

                return "Название не может быть загружено из-за исключения";
            }
        }

        public static string getArticleAuthor(string htmlText) {
            try
            {
                var author = "Автор не известен";
                var parser = new HtmlParser();
                var node = parser.Parse(htmlText);
                var authorFieldFinder = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                    && tag.Attributes.ContainsValue("material_topline_info"));
                node.AcceptVisitor(authorFieldFinder);

                if (authorFieldFinder.Result.Count > 0)
                {
                    var spanTag = (Majestic13.HtmlNode.Tag)authorFieldFinder.Result[0].Children[3];
                    var authorFinder = new FindTagsVisitor(tag => tag.Name == "a" && tag.Attributes.ContainsKey("href"));
                    spanTag.AcceptVisitor(authorFinder);
                    if (authorFinder.Result.Count > 0)
                    {
                        author = "";
                        for (int i = 0; i < authorFinder.Result.Count; i++)
                        {
                            var authorText = (Majestic13.HtmlNode.Text)authorFinder.Result[i].Children[0];
                            var authorToBeAdded = authorText.Value;
                            author = author + " " + authorToBeAdded;
                        }
                    }
                }
                else
                {
                    var authorFieldFinder2 = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                   && tag.Attributes.ContainsValue("autors_box"));
                    node.AcceptVisitor(authorFieldFinder2);
                    var aTag = (Majestic13.HtmlNode.Tag)authorFieldFinder2.Result[0].Children[2];
                    var aText = (Majestic13.HtmlNode.Text)aTag.Children[0];
                    author = aText.Value;
                }

                return author;
            }
            catch (Exception)
            {

                return "Автор не может быть загружен из-за исключения";
            }
            }

        public static DateTime getArticleDate(string htmlText)
        {
            try
            {
                var date = "Date not known";
                var parser = new HtmlParser();
                var node = parser.Parse(htmlText);
                var dateFieldFinder = new FindTagsVisitor(tag => tag.Name == "time");
                node.AcceptVisitor(dateFieldFinder);
                if (dateFieldFinder.Result.Count > 0)
                {
                    var timeTag = (Majestic13.HtmlNode.Tag)dateFieldFinder.Result[0];
                    var text = (Majestic13.HtmlNode.Text)timeTag.Children[0];
                    date = text.Value;
                }
                else
                {
                    var dateFieldFinder1 = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                        && tag.Attributes.ContainsValue("material_topline_info"));
                    node.AcceptVisitor(dateFieldFinder1);
                    var spanTag = (Majestic13.HtmlNode.Tag)dateFieldFinder1.Result[0].Children[1];
                    var text = (Majestic13.HtmlNode.Text)spanTag.Children[1];
                    date = text.Value;
                }
                if (date.Contains("сегодня")) return DateTime.Now;
                else return DateTime.ParseExact(date, "HH:mm dd/MM/yyyy", null);
            }
            catch (Exception)
            {

                return DateTime.Now;
            }   
            }

        public static string getArticleContent(string htmlText)
        {
            try
            {
                var content = "";
                var parser = new HtmlParser();
                var node = parser.Parse(htmlText);
                var contentFinder = new FindTagsVisitor(tag => tag.Name == "article");
                node.AcceptVisitor(contentFinder);
                var resultTag = new Majestic13.HtmlNode.Tag();
                resultTag = (Majestic13.HtmlNode.Tag)contentFinder.Result[0];
                content = ContinueUntilOnlyTextLeft(resultTag, content);
                LogServices.WriteProgressLog("Fetched content of article ");
                return content;
            }
            catch (Exception )
            {

                return "Содержимое не может быть загружено из-за исключения";
            }   
        }

        public static string ContinueUntilOnlyTextLeft(Majestic13.HtmlNode.Tag tag, string content) {
            var numberofchildren = tag.Children.Count;
            for (int i = 0; i < numberofchildren; i++) {
                var childtag = tag.Children[i];
                if (childtag is Majestic13.HtmlNode.Text)
                {
                    var text = (Majestic13.HtmlNode.Text)childtag;
                    content = content + " " + text.Value;
                }
               
                else
                {
                    if (childtag is  Majestic13.HtmlNode.Tag)
                    {
                        var tagToBeExtracted = (Majestic13.HtmlNode.Tag)childtag;
                        content = ContinueUntilOnlyTextLeft(tagToBeExtracted, content);
                    }
                }
                
            }
            return content; 
        }

        
        public static string getEditionName(string htmlText)
        {
            try
            {
                var parser = new HtmlParser();
                var node = parser.Parse(htmlText);
                var edNameFinder = new FindTagsVisitor(tag => tag.Name == "div" && tag.Attributes.ContainsKey("class")
                    && tag.Attributes.ContainsValue("newspaper_new"));
                node.AcceptVisitor(edNameFinder);
                if (edNameFinder.Result.Count > 0)
                {
                    var hrefTag = (Majestic13.HtmlNode.Tag)edNameFinder.Result[0].Children[3];
                    var edNameTag = (Majestic13.HtmlNode.Text)hrefTag.Children[0];
                    return edNameTag.Value;
                }
                else
                {
                    var edNameFinder1 = new FindTagsVisitor(tag => tag.Name == "span" && tag.Attributes.ContainsKey("class")
                    && tag.Attributes.ContainsValue("artic_num_box"));
                    node.AcceptVisitor(edNameFinder1);
                    var aTag = (Majestic13.HtmlNode.Tag)edNameFinder1.Result[1].Children[3];
                    var edNameTag = (Majestic13.HtmlNode.Text)aTag.Children[0];
                    return edNameTag.Value;
                }
            }
            catch (Exception)
            {

                return "Название не может быть загружено из-за исключения";
            }
        }

        public static List<Article> GetNewspapers(int Max) {           

                string CoreUrl = "http://www.aif.ru/gazeta/number/";
                var allArticles = new List<Article>();
                for (int i = 24355; i <= Max; i++)
                {
                    var newUrl = String.Concat(CoreUrl, i.ToString());
                    LogServices.WriteProgressLog("Formed new url " + newUrl);
                    var htmlText = GetWebText(newUrl);

                    if (htmlText != null)
                    {
                        if (isWeeklyNewspaper(htmlText))
                        {
                            var articlesToBeAdded = GetArticles(htmlText);
                            if (articlesToBeAdded != null)
                            {
                                LogServices.WriteProgressLog("Received list of urls ");
                                foreach (Article a in articlesToBeAdded)
                                {
                                    if (a.Url != null)
                                    {
                                        LogServices.WriteProgressLog("Preparing to add ");
                                        allArticles.Add(a);
                                        //add to database
                                        LogServices.WriteProgressLog("Trying to insert article to database");
                                        try
                                        {
                                            articleService.Insert(a);
                                            LogServices.WriteProgressLog("Successfully inserted to database");
                                        }
                                        catch (Exception)
                                        {
                                            LogServices.WriteProgressLog("Could not add this article to database" + a.Url);
                                        }                                       
                                         
                                    }
                                } 
                            }

                        }
                    }
                }
                return allArticles;
            
        }
        //After newspaper page where links to articles stored is downloaded, now possible to download html of articles
        //the whole html of page is passed
        public static List<Article> GetArticles (string htmlText){
            try
            {
                var articles = new List<Article>();
                var linksList = extractLinks(htmlText);
                LogServices.WriteProgressLog("Formed list of links" + linksList.ToString());
                foreach (string s in linksList)
                {
                    var article = CreateArticle(s);
                    LogServices.WriteProgressLog("Created new article");
                    articles.Add(article);
                    LogServices.WriteProgressLog("Added new article" + article.ToString());

                };
                return articles;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static Article CreateArticle(string url) {
            string htmlText = GetWebText(url);
            Article result = new Article();
            if (htmlText != null)
            {
                result.Url = url;

                if (!htmlText.Contains("Вопрос-ответ") || !htmlText.Contains("Ответ редакции"))
                {
                    result.isQuestionAnswer = "No";
                    result.Title = getArticleTitle(htmlText);
                    result.Author = getArticleAuthor(htmlText);
                    result.DatePublished = getArticleDate(htmlText);
                    result.Context = getArticleContent(htmlText);
                    result.EditionName = getEditionName(htmlText);
                }
                else {
                    result.isQuestionAnswer = "Yes";
                    result.Title = QAServices.getQATitle(htmlText);
                    result.EditionName = QAServices.getQAEditionName(htmlText);
                    result.Author = QAServices.getQAAuthor(htmlText);
                    result.DatePublished = QAServices.getQADate(htmlText);                
                    result.Context = QAServices.getQAContent(htmlText);                
                }
            }
            return result;  
        }

        

    }
}
