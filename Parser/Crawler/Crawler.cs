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
    public class Crawler
    {
        Dictionary<CrawlableSource, List<Event>> events;
        public Crawler()
        {
            events = new Dictionary<CrawlableSource, List<Event>>();
        }

        public async IAsyncEnumerable<Event> StartAsync(IEnumerable<CrawlableSource> sourcers)
        {
            var fromCrawbleSourceToEnumerator = new Dictionary<IAsyncEnumerator<Event>, CrawlableSource>();

            var newsEnumerators = sourcers.Select(x => 
            {
                if (events.ContainsKey(x))
                    x.LastCrawlableEvent = events[x].LastOrDefault();
                var enumerator = x.CrawlAsync().GetAsyncEnumerator();
                fromCrawbleSourceToEnumerator[enumerator] = x;
                return enumerator;
                })
                .ToList();

            var dictionary = new Dictionary<Task<bool>, IAsyncEnumerator<Event>>();
            var currentEvents = new List<Task<bool>>();
            var counter = 0;
            var startTime = DateTime.Now;

            while (newsEnumerators.Any())
            {
                foreach (var en in newsEnumerators)
                {
                    lock (dictionary)
                    {
                        var next = en.MoveNextAsync().AsTask();
                        currentEvents.Add(next);
                        dictionary[next] = en;
                    }
                }
                while (currentEvents.Any())
                {
                    var cur = await Task.WhenAny(currentEvents);
                    currentEvents.Remove(cur);
                    IAsyncEnumerator<Event> enumer;
                    lock (dictionary)
                    {
                        enumer = dictionary[cur];
                        //dictionary.Remove(cur);
                    }
                    var resCur = await cur;
                    if (resCur)
                    {
                        var nnn = enumer.Current;
                        try
                        {
                            var key = fromCrawbleSourceToEnumerator[enumer];
                            if (events.ContainsKey(key))
                                events[key].Add(nnn);
                            else
                                events[key] = new List<Event>() { nnn};

                        }
                        catch(KeyNotFoundException e)
                        {
                            Debug.Print("нет переходного ключа");
                        }
                        yield return nnn;
                        counter++;
                    }
                    else
                    {
                        newsEnumerators.Remove(enumer);
                    }
                }
            }
        }
       
    }
}
