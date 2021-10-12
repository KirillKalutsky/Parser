using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class PageArchitectureSite : Website, ICrawlable
    {
        public PageArchitectureSite(HttpClient httpClient) : base(httpClient) { }

        public string LinkURL { get; set; }
        public HtmlElement LinkElement { get; set; }
        public Dictionary<string, string> ParseEventProperties { get; set; }

        public async IAsyncEnumerable<Event> CrawlAsync()
        {
            var newsCounter = 0;
            var document = new HtmlDocument();
            var pageCounter = 1;
            var url = $"{StartURL}{pageCounter}/";

            while (newsCounter < 1000)
            {

                var page = await PageLoader.LoadPage(url);
                if (!page.Item1.IsSuccessStatusCode)
                    yield break;

                var links = await GetNewsLinks(url, LinkElement);
                var pages = links.Select(x => PageLoader.LoadPage($"{LinkURL}{x}")).ToList();
                while (pages.Any())
                {
                    var tP = await Task.WhenAny(pages);
                    pages.Remove(tP);
                    var p = await tP;
                    document.LoadHtml(await p.Item1.Content.ReadAsStringAsync());
                    var news = NewsParser.ParseHtmlPage(document, ParseEventProperties);
                    news.Link = p.Item2;
                    yield return news;
                    newsCounter++;
                }

                pageCounter += 1;
                url = $"{StartURL}{pageCounter}/";
            }
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
    }
}
