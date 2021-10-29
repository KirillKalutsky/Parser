using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public abstract class Website 
    {

        private readonly HttpClient httpClient;
        public Website(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public string StartURL { get; set; }



        //public Func<HtmlDocument, Dictionary<string, string>, Event> Parser { get; set; }
    }
}
