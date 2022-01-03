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
        private readonly string defaultCategory;
        private readonly char[] splitSymbols = new char[] { ',', '.', ' ', '\'', '\"', '\t', '\n', '?', '!' };
        public Analyzer(Dictionary<string, HashSet<string>> categories, string defaultCategory)
        {
            this.defaultCategory = defaultCategory;
            this.categories = categories;
            var dataFilepath = @"D:\c#\RedZone\full7z-mlteast-ru.lem";
            FileStream stream = File.OpenRead(dataFilepath);
            lemmatizer = new Lemmatizer(stream);
            stream.Close();
        }

        public Task<string> AnalizeCategoryAsync(string text)
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

        private IEnumerable<string> SplitTextToTokens(string text)
        {
            return text.ToLower()
                .Split(splitSymbols)
                .Where(token=>!(string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token)));
        }
    }
}
