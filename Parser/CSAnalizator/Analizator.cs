using LemmaSharp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.CSAnalizator
{
    public class Analizator
    {
        private Dictionary<string, HashSet<string>> categories;
        private readonly Lemmatizer lemmatizer;
        public Analizator(Dictionary<string, HashSet<string>> categories)
        {
            this.categories = categories;
            var dataFilepath = @"D:\c#\RedZone\full7z-mlteast-ru.lem";
            FileStream stream = File.OpenRead(dataFilepath);
            lemmatizer = new Lemmatizer(stream);
        }

        public string Analize(string text, string defaultCategory)
        {
            var result = new Dictionary<string, int>();

            var tokens = SplitTextToTokens(text);
            foreach(var token in tokens)
            {
                foreach(var val in categories)
                {
                    if(val.Value.Contains(token))
                    {
                        var word = lemmatizer.Lemmatize(val.Key);
                        if (result.ContainsKey(word))
                            result[word]++;
                        else
                            result[word] = 1;
                    }
                }
            }

            if (!result.Keys.Any())
                return defaultCategory;

            return result.OrderBy(val => val.Value).Last().Key;
        }

        private List<string> SplitTextToTokens(string text)
        {
            var tokens = text.ToLower()
                .Split(new char[] {',','.',' ','\'','\"','\t','\n','?','!' })
                .Where(token=>!(string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token)));
            return tokens.ToList();
        }
    }
}
