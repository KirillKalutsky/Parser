using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    //public readonly string token = "07c4a15607c4a15607c4a156c107bc638c007c407c4a15666c63002c593e62fc9c5e1c2";
    public class VKGroup
    {
        HttpClient httpClient;
        public VKGroup(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        //$"https://api.vk.com/method/wall.get?{domain={domain}&count=100&offset={count}}&access_token={vk.token}&v={v}"
        //v = "5.131" - версия вк апи
        private string domain;//краткое название группы
        private int count;//число загруженных постов

        public IAsyncEnumerable<Event> CrawlAsync()
        {
            throw new NotImplementedException();
        }

        public bool IsScanned(string url)
        {
            throw new NotImplementedException();
        }
    }
}
