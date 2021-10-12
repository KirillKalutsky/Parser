using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Crawler
    {
        readonly HttpClient httpClient;
        public Crawler()
        {
            httpClient = new HttpClient();
        }

        public async Task<IEnumerable<string>> GetNewsLinks(string url, HtmlElement link)
        {
            var links = new HashSet<string>();

            var body = (await PageLoader.LoadPage(url)).Item1;

            if (body.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await body.Content.ReadAsStringAsync());

                var searchElements = doc.DocumentNode.SelectNodes(link.XPath);

                foreach (var e in searchElements)
                    links.Add(e.GetAttributeValue(link.AttributeName, ""));
            }
            return links;
        }

        public async IAsyncEnumerable<Event> GetNews(string url, string mainURL,HtmlElement link, HtmlElement nextPageButton,Dictionary<string,string> eventProperties)
        {
            var newsCounter = 0;
            var document = new HtmlDocument();

            while (newsCounter < 1000)
            {
                /*var page = (await PageLoader.LoadPage(url)).Item1;
                if (!page.IsSuccessStatusCode)
                    yield return new Event();*/

                var links = await GetNewsLinks(url,link);
                var pages = links.Select(x=>PageLoader.LoadPage($"{mainURL}{x}")).ToList();
                while (pages.Any())
                {
                    var tP = await Task.WhenAny(pages);
                    pages.Remove(tP);
                    var p = await tP;
                    document.LoadHtml(await p.Item1.Content.ReadAsStringAsync());
                    var news = NewsParser.ParseHtmlPage(document, eventProperties);
                    news.Link = p.Item2;
                    yield return news;
                    newsCounter++;
                }

                url = $"{mainURL}{await GetNextPageLink(url, nextPageButton)}";
            }
        }

        public async Task<string> GetNextPageLink(string url, HtmlElement nextPageButton)
        {
            var body = (await PageLoader.LoadPage(url)).Item1;
            var result = "";
            if (body.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await body.Content.ReadAsStringAsync());

                var searchElements = doc.DocumentNode.SelectNodes(nextPageButton.XPath);

                result = searchElements.FirstOrDefault().GetAttributeValue(nextPageButton.AttributeName, "");
            }
            return result;
        }
    }
}
