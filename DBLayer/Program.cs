using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using Parser;
using Parser.Python;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBLayer
{
    
    public class Setter
    {
        public string SourceType { get; }
        public Setter(CrawlableSource source)
        {
            SourceType = source.GetType().Name;
            Source = source;
        }

        public CrawlableSource Source { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Source);
        }

        public PageArchitectureSite ToSource(string json)
        {
            return JsonConvert.DeserializeObject<PageArchitectureSite>(json);
        }
    }

    
        
    class Program
    {
        

        static async Task Main(string[] args)
        {


            var dbContext = new MyDBContext();

            Console.WriteLine(dbContext.Events.Where(x=>x.Link== "https://veved.ru/eburg/news/life/169176-ekaterinburgskogo-avtohama-priznali-vinovnym-v-ugroze-ubijstvom.html").Count());

            var newEvent = new Event { Link = "https://veved.ru/eburg/news/life/169176-ekaterinburgskogo-avtohama-priznali-vinovnym-v-ugroze-ubijstvom.html" };
            //if(!dbContext.Events.Where(x=>x.Link==newEvent.Link).Any())
            try
            {
                await dbContext.Events.AddAsync(new Event { Link = "https://veved.ru/eburg/news/life/169176-ekaterinburgskogo-avtohama-priznali-vinovnym-v-ugroze-ubijstvom.html" });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(dbContext.Events.Where(x => x.Link == "https://veved.ru/eburg/news/life/169176-ekaterinburgskogo-avtohama-priznali-vinovnym-v-ugroze-ubijstvom.html").Count());

            dbContext.SaveChanges();


            /*var crawler = new Crawler();

            var curEvents = await dbContext.GetAllEventsAsync();
            Console.WriteLine(curEvents.Count);
            foreach (var e in curEvents)
                Console.WriteLine(e.Link);


            var sources = await dbContext.GetSourcesAsync();

            foreach (var s in sources)
                Console.WriteLine($"{s.Events.Count} {s.Fields.Properties}");
            var counter = 0;
            await foreach (var e in crawler.StartAsync(sources))
            {
                counter++;
                Console.WriteLine(counter.ToString());
                Console.WriteLine(e.Link);
                await dbContext.AddEventAsync(e);
            }

            dbContext.SaveChanges();*/

            #region
            /*List<CrawlableSource> sourceList = new List<CrawlableSource>()
             {
                   

             *//*  //only bad status code
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

            /* //Робит(страница не очень красиво парсится)
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
              },*/

            /*new PageArchitectureSite()
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
                 EndUrl="/?year=&month=&day=&type=main",
                 LinkElement = new HtmlElement
                 {
                     XPath = @".//div[@class='bl-item-image']/a",
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
              },*//*

             //Робит
             *//*  new PageArchitectureSite()
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
              },*//*

          };

             var crawler = new Crawler();
             var set = new HashSet<string>();
             var counter = 1;
             await foreach (var s in crawler.StartAsync(sourceList))
             {
                 Console.WriteLine(counter);
                 counter++;
                 set.Add(s.Link);
                 Console.WriteLine(s.Body);
             }

             Console.WriteLine(set.Count);
             foreach (var l in set)
                 Console.WriteLine(l);*/

            /* var sources = new List<Source>
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
                                      { "Date", ".//span[@class='published']" }
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
                                      { "Date", ".//div[@class='bl-item-date m_b2']" }
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
                                      { "Date", ".//div[@class='article-date-item']" }
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
                              }
                         )
                     }

                 },

             };*/
            #endregion

        }



    }

   
}
