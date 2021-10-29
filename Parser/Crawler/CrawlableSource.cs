using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public abstract class CrawlableSource
    {
        public abstract IAsyncEnumerable<Event> CrawlAsync();
        public  bool IsLastLinkToEvent(string url)
        {   
            if (LastCrawlableEvent == null) return false;
            return url.Equals(LastCrawlableEvent.Link);
        }
        public Event LastCrawlableEvent { get; set; }

       
    }
}
