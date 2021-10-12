using System;
using System.Collections.Generic;
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

            #region

            var httpClient = new HttpClient();

            //Робит
            var website = new PageArchitectureSite(httpClient)
            {
                StartURL = "https://sverdlovsk.sledcom.ru/Novosti/",
                LinkURL = "https://sverdlovsk.sledcom.ru",
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
            };

            //Робит
            var website1 = new PageArchitectureSite(httpClient)
            {
                StartURL = "https://66.xn--b1aew.xn--p1ai/news/1",
                LinkURL = "https://66.xn--b1aew.xn--p1ai",
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
            };

            //Робит
            var website2 = new PageArchitectureSite(httpClient)
            {
                StartURL = "https://eburg.mk.ru/news/",
                LinkURL = "",
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
            };

            //не знаю как указать на следующую страницу
            var website3 = new PageArchitectureSite(httpClient)
            {
                StartURL = "https://ekaterinburg.bezformata.com/incident/?npage=",
                LinkURL = "",
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
            };

           /* //подгружается по кнопке, но ссылка хз где
            var website4 = new WebsiteWithLinksInElement(httpClient)
            {
                StartURL = "https://www.justmedia.ru/news/events",
                LinkURL = "https://www.justmedia.ru",
                NextButtonURL = "https://ekaterinburg.bezformata.com",
                LinkElement = new HtmlElement
                {
                    XPath = @".//p[@class='news-one__desc']/a",
                    AttributeName = "href"
                },
                NextPageButtonElement = new HtmlElement
                {
                    XPath = @".//ul[@id='nav-pages']",
                    AttributeName = "href"
                },
                ParseEventProperties = new Dictionary<string, string>
                {
                    { "Body", ".//div[@class='article__body']//p" },
                    { "Date", ".//time[@class='meta__text']" }
                }
            };*/

            //падает
            var website5 = new PageArchitectureSite(httpClient)
            {
                StartURL = "https://veved.ru/eburg/news/page/",
                LinkURL = "",
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
            };



            var newsCounter = 0;
            await foreach (var n in website3.CrawlAsync())
            {
                Console.WriteLine(newsCounter);
                Console.WriteLine($"Link: {n.Link}");
                Console.WriteLine($"Main Data: {n.Body}");
                Console.WriteLine($"Date: {n.Date}");
                Console.WriteLine();
                newsCounter++;
            }
            #endregion
        }
    }
}
