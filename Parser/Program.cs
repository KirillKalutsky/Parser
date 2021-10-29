using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Parser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region
            /*var news = new List<Event>();
            var crawler = new Crawler();
            var linkSled = "https://sverdlovsk.sledcom.ru/Novosti";
            var Sled = "https://sverdlovsk.sledcom.ru";

            var counter = 0;

            while (counter < 1000)
            {
                var firstPageLinks = await crawler.GetNewsLinks
                (
                    linkSled,
                    new HtmlElement
                    {
                        XPath = @".//div[@class='bl-item clearfix']//a",
                        AttributeName = "href"
                    }
                );

                counter++;

                foreach (var link in firstPageLinks)
                    Console.WriteLine($"{link}{counter}");

                linkSled = $"{Sled}{await crawler.GetNextPageLink(linkSled, new HtmlElement { XPath = @".//a[@class='bp-next']", AttributeName = "href" })}";
            }


            var secondPageLink = $"{Sled}{await crawler.GetNextPageLink(linkSled, new HtmlElement { XPath = @".//a[@class='bp-next']", AttributeName = "href" })}";

            Console.WriteLine(secondPageLink);

            var secondPageLinks = await crawler.GetNewsLinks
                (
                    secondPageLink,
                    new HtmlElement
                    {
                        XPath = @".//div[@class='bl-item clearfix']//a",
                        AttributeName = "href"
                    }
                );

            foreach (var link in secondPageLinks)
                Console.WriteLine(link);*/
            #endregion

            #region
            /*var page = await PageLoader.LoadPage("https://www.e1.ru/text/incidents/2021/10/08/70180877/");

            if (page.IsSuccessStatusCode)
            {
                var content = page.Content;

                var document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(await content.ReadAsStringAsync());

                var text = NewsParser.Parse(
                    document,
                    new Dictionary<string, string> { { "Body", ".//div[@class='I7mv']//p" } }
                    );
                Console.WriteLine(text.Body);
            }*/

            #endregion

            //новости на страницах
            #region

            var httpClient = new HttpClient();

            var sourceList = new List<CrawlableSource>()
             {
                    //постонно меняются классы на странице(не находятся ссылки на статьи)
                 /*new PageArchitectureSite(httpClient)
                 {
                     StartUrl = "https://www.e1.ru/text/?page=",
                     LinkURL = "https://www.e1.ru",
                     EndUrl="",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//article[@class='HJak7']/a",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//div[@itemprop='articleBody']" },
                         { "Date", ".//time" }
                     }
                 },*/

                /*   //only bad status code
                 new PageArchitectureSite()
                 {
                     StartUrl = "https://ekaterinburg.bezformata.com/incident/?npage=",
                     LinkURL = "",
                     EndUrl="",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//article[@class='listtopicline']/a",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//div[@class='article__body']//p" },
                         { "Date", ".//time[@class='meta__text']" }
                     }
                 },*/

                //Робит(страница не очень красиво парсится)
                 new PageArchitectureSite()
                 {
                     StartUrl = "https://veved.ru/eburg/news/page/",
                     LinkURL = "",
                     EndUrl="",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//a[@class='box']",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//div[@class='fullstory-column']" },
                         { "Date", ".//div[@class='vremya']" }
                     }
                 },
                
                /* new PageArchitectureSite()
                 {
                     StartUrl = "https://ural-meridian.ru/news/category/sverdlovskaya-oblast/page/",
                     LinkURL = "",
                     EndUrl="/",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//h2[@class='entry-title']/a",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//div[@class='entry-content clear']" },
                         { "Date", ".//span[@class='published']" }
                     }
                 },*/

                /* //Робит
                 new PageArchitectureSite()
                 {
                     StartUrl = "https://sverdlovsk.sledcom.ru/Novosti/",
                     LinkURL = "https://sverdlovsk.sledcom.ru",
                     EndUrl="",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//div[@class='bl-item clearfix']//a",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//article[@class='c-detail m_b4']//p" },
                         { "Date", ".//div[@class='bl-item-date m_b2']" }
                     }
                 },*/

                /* //Робит
                 new PageArchitectureSite()
                 {
                     StartUrl = "https://66.xn--b1aew.xn--p1ai/news/1",
                     LinkURL = "https://66.xn--b1aew.xn--p1ai",
                     EndUrl="",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//div[@class='sl-item-title']/a",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//div[@class='article']//p" },
                         { "Date", ".//div[@class='article-date-item']" }
                     }
                 },*/

               /* //Робит
                 new PageArchitectureSite()
                 {
                     StartUrl = "https://eburg.mk.ru/news/",
                     LinkURL = "",
                     EndUrl="/",
                     LinkElement = new HtmlElement
                     {
                         XPath = @".//a[@class='news-listing__item-link']",
                         AttributeName = "href"
                     },

                     ParseEventProperties = new Dictionary<string, string>
                     {
                         { "Body", ".//div[@class='article__body']//p" },
                         { "Date", ".//time[@class='meta__text']" }
                     }
                 },*/
                
             };




            /*var newsCounter = 0;

            foreach (var e in sourceList)
            {
                await foreach (var n in e.CrawlAsync())
                {
                    newsCounter++;
                    Console.WriteLine(newsCounter);
                    Console.WriteLine(n.Link);
                    Console.WriteLine(n.Body);
                    Console.WriteLine();
                }
            }*/

            
            var startTime = DateTime.Now;

            var crawler = new Crawler();
            var counter = 1;
            await foreach (var e in crawler.StartAsync(sourceList))
            {
                //Console.WriteLine(counter);
                //counter += 1;
            }
                

            Console.Write(DateTime.Now - startTime);

            #endregion
/*
            HttpClient client = new HttpClient();

            var url = "https://regnum.ru/api/get/geography/1/0/5/undefined/81/1633521636";
            var json = new WebClient().DownloadString(url);
            Console.WriteLine(json);
            var res = await client.GetAsync(url);
            Console.WriteLine(res.IsSuccessStatusCode);
            var document = new HtmlDocument();
            var content = await res.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            document.LoadHtml(content);

            var list = document.DocumentNode.SelectNodes("a");
            foreach(var e in list)
            {
                Console.WriteLine(e.InnerText);
                Console.WriteLine(e.GetAttributeValue("href", ""));
                Console.WriteLine();
            }*/
        }

    }
}
