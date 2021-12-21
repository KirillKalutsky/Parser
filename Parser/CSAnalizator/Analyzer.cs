using DeepMorphy;
using LemmaSharp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.CSAnalizator
{
    public class Analyzer
    {
        private Dictionary<string, HashSet<string>> categories;
        private readonly Lemmatizer lemmatizer;
        public Analyzer(Dictionary<string, HashSet<string>> categories, MorphAnalyzer morphAnalyzer)
        {
            this.categories = categories;
            var dataFilepath = @"D:\c#\RedZone\full7z-mlteast-ru.lem";
            FileStream stream = File.OpenRead(dataFilepath);
            lemmatizer = new Lemmatizer(stream);
        }

        public Task<string> AnalizeCategoryAsync(string text, string defaultCategory)
        {
            return Task.Run(() =>
            {
                var result = new Dictionary<string, int>();

                var tokens = SplitTextToTokens(text);
                foreach (var token in tokens)
                {
                    foreach (var val in categories)
                    {
                        var word = lemmatizer.Lemmatize(token);
                        if (val.Value.Contains(word))
                        {
                            if (result.ContainsKey(val.Key))
                                result[val.Key]++;
                            else
                                result[val.Key] = 1;
                        }
                    }
                }

                if (!result.Keys.Any())
                    return defaultCategory;

                return result.OrderBy(val => val.Value).Last().Key;
            });
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
