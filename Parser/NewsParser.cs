using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class NewsParser
    {
        public static Event ParseHtmlPage(HtmlDocument page, Dictionary<string,string> pageElements) 
        {
            var result = new Event();
            foreach(var e in pageElements)
            {
                try
                {
                    var p = result.GetType().GetProperty(e.Key);
                    var value = new StringBuilder();
                    foreach (var node in page.DocumentNode.SelectNodes(e.Value))
                        value.Append(node.InnerText);
                    p.SetValue(result, value.ToString());
                    
                }
                catch { }
            }
            return result;
        }
    }
}
