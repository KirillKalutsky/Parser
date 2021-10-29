using Newtonsoft.Json;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public static class MyDBInitializer
    {
        public static  void Init(MyDBContext context)
        {
            if (context.Sources.Any())
                return;

            #region
            /*List<CrawlableSource> sourceList = new List<CrawlableSource>()
             {
                    //постонно меняются классы на странице(не находятся ссылки на статьи)
                 *//*new PageArchitectureSite(httpClient)
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
                 },*//*

                   //only bad status code
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
                 },

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

                 new PageArchitectureSite()
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
                 },

                 //Робит
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
                 },

                 //Робит
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
                 },

                //Робит
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
                 },

             };*/
            #endregion

            var sources = new List<Source>
            {
                new Source()
                {
                    SourceType = SourceType.PageSite,
                    Fields = new SourceFields()
                    {
                        Properties = JsonConvert.SerializeObject
                        (
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
                             }
                        )
                    }

                }
            };

            foreach (var source in sources)
                context.Sources.Add(source);

            context.SaveChanges();
        }
    }
}
