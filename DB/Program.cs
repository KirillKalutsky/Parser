using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using Parser;
using Parser.Python;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Parser.CSAnalizator;
using LemmaSharp;
using LemmaSharp.Classes;
using DeepMorphy;
using Parser.Infrastructure.Python;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DB
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public Person Parent { get; set; }
    }

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


    public class Try<T>
    {
        public Try(Func<T,string> func)
        {
            Print = func;
        }
        private Func<T, string> Print;
        private Func<T, string> GetPrint()
        {
            return Print;
        }
    }

   

    class Program
    {
        public static Expression<Func<TOwner, TPropType>> Excluding<TOwner, TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            return memberSelector;
        }

        
        public static void UnPush(object obj, Dictionary<object,Tuple<Type,Type>> dict)
        {
            var t = dict[obj];

            Console.WriteLine(t.Item1);
            Console.WriteLine(t.Item2);

            var f = (Func< Person,string>)obj;
            Console.WriteLine(f.Invoke(new Person() { Name = "Tttoha", Age = 33, Height = 1.5 }));
        }

        static string GetNameFromMemberExpression(Expression expression) {
		if (expression is MemberExpression) {
			return (expression as MemberExpression).Member.Name;
		}
		else if (expression is UnaryExpression) {
			return GetNameFromMemberExpression((expression as UnaryExpression).Operand);
		}
		return "MemberNameUnknown";
	    }

        public static string GetInfo<T, P>(Expression<Func<T, P>> action)
        {
            var expression = (MemberExpression)action.Body;
            string name = expression.Member.Name;

            return name;
        }

        private static string PrintToString(string parentPath, object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();


            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                string newPath;
                if (string.IsNullOrEmpty(parentPath))
                    newPath = $"{propertyInfo.Name}";
                else
                    newPath = $"{parentPath}.{propertyInfo.Name}";
                Console.WriteLine(newPath);

                sb.Append(identation + propertyInfo.Name + " = " +
                        PrintToString(newPath, propertyInfo.GetValue(obj),
                            nestingLevel + 1));

            }

            return sb.ToString();
        }

        /*private static void PrintToString(string parentPath, object obj, int nestingLevel)
        {
            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (obj == null || finalTypes.Contains(obj.GetType()))
            {
                return;
            }

            var type = obj.GetType();
            foreach (var propertyInfo in type.GetProperties())
            {
                string newPath;
                if(string.IsNullOrEmpty(parentPath))
                    newPath = $"{propertyInfo.Name}";
                else
                    newPath = $"{parentPath}.{propertyInfo.Name}";
                Console.WriteLine(newPath);
                PrintToString(newPath, propertyInfo.GetValue(obj), nestingLevel + 1);
            }
        }*/


        public static string Foo<T, P>(Expression<Func<T, P>> expr)
        {
            var result = new Stack<string>();
            MemberExpression me = (MemberExpression)expr.Body;

            while (me != null)
            {
                string propertyName = me.Member.Name;

                result.Push(propertyName);

                me = me.Expression as MemberExpression;
            }

            return string.Join(".", result);
        }


        static async Task Main(string[] args)
        {



            /* var districts = new List<string>()
             {
                 "академический",
                 "верх-исетский",
                 "железнодорожный",
                 "кировский",
                 "ленинский",
                 "октябрьский",
                 "орджоникидзевский",
                 "чкаловский"
             };
             //var morph = new MorphAnalyzer(withLemmatization: true, withTrimAndLower: true);



             var categories = new Dictionary<string, HashSet<string>>()
                 {
                     {"ДТП" , new HashSet<string>() {"протаранить", "проехать", "наехать", "сбить", "выехать", "вылететь", "столкнуться", "врезаться", "авария" , "автомобиль"}},
                     {"Пожар" , new HashSet<string>() {"сгореть", "загореться" , "огонь", "пламя", "потушить", "воспламяниться", "гореть", "пожар", "вспыхнуть", "взорваться", "возгорание", "задымление", "дым", "газ"}},
                     {"Криминал" , new HashSet<string>() {"украсть", "ограбить", "ударить", "избить", "убить", "убийство", "кража", "труп", "тело" }},
                 };
             var analizator = new Analizator(categories, new MorphAnalyzer(withLemmatization: true, withTrimAndLower: true));
             var category = await analizator.AnalizeCategoryAsync("Саня сосет бибу", "Не ЧП");

             Console.WriteLine(category);*/

            var dataFilepath = @"..\..\..\..\Parser\CSAnalizator\streets_list_with_districts.csv";
            Console.WriteLine(Path.GetFullPath(dataFilepath));
            FileStream stream = File.OpenRead(dataFilepath);
            var lemmatizer = new Lemmatizer(stream);

            var words = @"авария сетях прорыв сети магистральные трубопровод водоснабжение водоснабжения холодное ХВС горячее ГВС вода коммунальная затопление затопления затоплению теплоснабжение теплоснабжения отключение теплоснабжение ремонтные отопление отключено отопление отопления трубопровод водоснабжения трубы разрыв водовод водоснабжение отключена перекрыть перекрытие электроснабжение электроснабжения электричество электричества кабельные повредили повреждение свет электроснабжения электричество нарушение нарушено воздушной электричества пропало распределительная сеть распределительной подземный кабельно-воздушная линии электросетевой комплекс"; 

            var ws = words.Split(" ");

            var hash = new HashSet<string>();
            foreach (var w in ws)
                hash.Add(lemmatizer.Lemmatize(w));

            foreach (var w in hash)
                Console.WriteLine(w);
            /*using (var reader = new StreamReader(@"D:\c#\RedZone\CH.csv"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    foreach (var val in values)
                        Console.Write($"{val} ");
                    Console.WriteLine();
                }
            }*/


            /* var csvTable = new DataTable();
             using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(@"D:\c#\RedZone\districs\streets_list_with_districts.csv")), true))
             {
                 csvTable.Load(csvReader);
             }


             for (int i = 0; i < csvTable.Rows.Count; i++)
             {
                 Console.WriteLine($"{csvTable.Rows[i][0]} ");
             }

             Console.WriteLine(csvTable.Rows.Count);
             Console.WriteLine(csvTable.Columns.Count);*/


            /* var dir = new Dictionary<object, Tuple<Type, Type>>();

             var f = Excluding<Person, string>(x => x.Name).Compile();

             dir[f] = Tuple.Create(typeof(Person), typeof(string));

             UnPush(f, dir);*/

            /*Console.WriteLine(dir[f]);

            var r = f.Compile();
            var person = new Person() { Name = "Toha", Age = 22 };
            var personProperty = person.GetType().GetProperties().Where(x => x.Name != f.Name);

            var exceptProperty = r.Invoke(person);

            Console.WriteLine(f.GetType().Name);
            foreach(var p in personProperty)
            {
                Console.WriteLine(p.Name);
            }*/




            /*  System.Globalization.CultureInfo EnglishCulture = new
  System.Globalization.CultureInfo("en-EN");
              System.Globalization.CultureInfo GermanCulture = new
              System.Globalization.CultureInfo("de-de");

              double val;
              if (double.TryParse("65,89875", System.Globalization.NumberStyles.Float,
              GermanCulture, out val))
              {
                  string valInGermanFormat = val.ToString(GermanCulture);
                  string valInEnglishFormat = val.ToString(EnglishCulture);
                  Console.WriteLine(valInGermanFormat);
                  Console.WriteLine(valInEnglishFormat);
              }

              if (double.TryParse("65.89875", System.Globalization.NumberStyles.Float,
              EnglishCulture, out val))
              {
                  string valInGermanFormat = val.ToString(GermanCulture);
                  string valInEnglishFormat = val.ToString(EnglishCulture);
                  Console.WriteLine(valInGermanFormat);
                  Console.WriteLine(valInEnglishFormat);
              }*/
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
