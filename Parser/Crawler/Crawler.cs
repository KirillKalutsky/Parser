using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Crawler
    {
        public async IAsyncEnumerable<Event> StartAsync(IEnumerable<Source> sourcers)
        {
            var sourceEnumerators = GetCrawbleSourceEnumerator(sourcers).ToList();

            while (sourceEnumerators.Any())
            {
                
                foreach (var en in sourceEnumerators)
                {
                    var next = await en.MoveNextAsync().AsTask();
                    if (next)
                        yield return en.Current;
                    else
                        sourceEnumerators.Remove(en);
                }
            }
        }
        IEnumerable<IAsyncEnumerator<Event>> GetCrawbleSourceEnumerator(IEnumerable<Source> sourcers)
        {
            var fromCrawbleSourceToEnumerator = new LinkedList<IAsyncEnumerator<Event>>();

            foreach (var s in sourcers)
            {
                var source = GetCrawlableSourceFromSource(s);
                var enumerator = source.CrawlAsync(s).GetAsyncEnumerator();
                fromCrawbleSourceToEnumerator.AddLast(enumerator);
            }

            return fromCrawbleSourceToEnumerator;
        }

        private CrawlableSource GetCrawlableSourceFromSource(Source s)
        {
            CrawlableSource crawbleSource;
            switch (s.SourceType)
            {
                case SourceType.PageSite:
                    var pageSite = JsonConvert.DeserializeObject<PageArchitectureSite>(s.Fields.Properties);
                    if (s.Events != null)
                        pageSite.LastEvent = s.Events.OrderBy(x => x.DateOfDownload).LastOrDefault();
                    crawbleSource = pageSite;
                    break;
                default:
                    var site = JsonConvert.DeserializeObject<PageArchitectureSite>(s.Fields.Properties);
                    if (s.Events != null)
                        site.LastEvent = s.Events.OrderBy(x => x.DateOfDownload).LastOrDefault();
                    crawbleSource = site;
                    break;
            }
            return crawbleSource;
        }
    }
}
