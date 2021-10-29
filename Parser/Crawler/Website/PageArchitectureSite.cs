using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class PageArchitectureSite : CrawlableSource
    {
        public PageArchitectureSite() 
        { }

        
        HashSet<string> links = new HashSet<string>();
        public string StartUrl { get; set; }
        public string EndUrl { get; set; }
        public string LinkURL { get; set; }
        public HtmlElement LinkElement { get; set; }
        public Dictionary<string, string> ParseEventProperties { get; set; }


        public override async IAsyncEnumerable<Event> CrawlAsync()
        {
            var newsCounter = 0;
            var pageCounter = 1;
            var url = $"{StartUrl}{pageCounter}{EndUrl}";
            var repetition = false;
            while (!repetition || newsCounter<50)
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

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(await p.Item1.Content.ReadAsStringAsync());

                    Event news = new Event();
                    
                    news = NewsParser.ParseHtmlPage(document, ParseEventProperties);
                    
                    news.Link = p.Item2;
                    repetition = IsLastLinkToEvent(news.Link);

                    yield return news;
                    newsCounter+=1;
                }

                pageCounter += 1;
                url = $"{StartUrl}{pageCounter}{EndUrl}";
            }
        }

        

        public async Task<IEnumerable<string>> GetNewsLinks(string url, HtmlElement link)
        {
            var result = new List<string>();

            var body = (await PageLoader.LoadPage(url)).Item1;

            if (body.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await body.Content.ReadAsStringAsync());

                var searchElements = doc.DocumentNode.SelectNodes(link.XPath);

                if (searchElements != null)
                    foreach (var e in searchElements)
                    {
                        var l = e.GetAttributeValue(link.AttributeName, "");
                        if (!links.Contains(l))
                        {
                            links.Add(l);
                            result.Add(l);
                        }

                    }
                else
                    Console.WriteLine($"{url} : нет ссылок на статьи");
            }
            return result;
        }

    }
}
