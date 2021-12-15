using Newtonsoft.Json;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public static class MyDBInitializer
    {
        public static  void Init(MyDBContext context)
        {
            if (context.Sources.Any())
                return;

            #region
             List<Source> sources = new List<Source>
        {
            //повторение идет
            new Source()
                {
                    SourceType = SourceType.PageSite,
                    Fields = new SourceFields()
                    {
                        Properties = JsonConvert.SerializeObject
                        (
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
                                 { "Date", ".//div[@class='vremya']" },
                                 { "Title", ".//h1[@class='zagolovok1']" }
                             }
                         }
                        )
                    }

                },
            new Source()
            {
                SourceType = SourceType.PageSite,
                Fields = new SourceFields()
                {
                    Properties = JsonConvert.SerializeObject
                    (
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
                                    { "Body", ".//div[@class='entry-content clear']/p" },
                                    { "Date", ".//span[@class='published']" },
                                    { "Title", ".//h1[@class='ast-advanced-headers-title']" }
                                }
                            }
                    )
                }

            },
            new Source()
            {
                SourceType = SourceType.PageSite,
                Fields = new SourceFields()
                {
                    Properties = JsonConvert.SerializeObject
                    (
                        new PageArchitectureSite()
                            {
                                StartUrl = "https://sverdlovsk.sledcom.ru/Novosti/",
                                LinkURL = "https://sverdlovsk.sledcom.ru",
                                EndUrl="/?year=&month=&day=&type=main",
                                LinkElement = new HtmlElement
                                {
                                    XPath = @".//div[@class='bl-item-image']/a",
                                    AttributeName = "href"
                                },

                                ParseEventProperties = new Dictionary<string, string>
                                {
                                    { "Body", ".//article[@class='c-detail m_b4']//p" },
                                    { "Date", ".//div[@class='bl-item-date m_b2']" },
                                    { "Title", ".//h1[@class='b-topic t-h1 m_b4']" }
                                }
                            }
                    )
                }

            },
            new Source()
            {
                SourceType = SourceType.PageSite,
                Fields = new SourceFields()
                {
                    Properties = JsonConvert.SerializeObject
                    (
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
                                { "Date", ".//div[@class='article-date-item']" },
                                { "Title", ".//div[@class='ln-content-holder']/h1" }
                            }
                        }
                    )
                }

            },
            //повторение идет
            new Source()
            {
                SourceType = SourceType.PageSite,
                Fields = new SourceFields()
                {
                    Properties = JsonConvert.SerializeObject
                    (
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
                                    { "Date", ".//time[@class='meta__text']" },
                                    { "Title", ".//h1[@class='article__title']" }
                                }
                            }
                    )
                }

            }
        };
        #endregion

        var sourceslist = new List<Source>
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
                                 StartUrl = "https://sverdlovsk.sledcom.ru/Novosti/",
                                 LinkURL = "https://sverdlovsk.sledcom.ru",
                                 EndUrl="/?year=&month=&day=&type=main",
                                 LinkElement = new HtmlElement
                                 {
                                     XPath = @".//div[@class='bl-item-image']/a",
                                     AttributeName = "href"
                                 },

                                 ParseEventProperties = new Dictionary<string, string>
                                 {
                                     { "Body", ".//article[@class='c-detail m_b4']//p" },
                                     { "Date", ".//div[@class='bl-item-date m_b2']" },
                                     { "Title", ".//h1[@class='b-topic t-h1 m_b4']" }
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
