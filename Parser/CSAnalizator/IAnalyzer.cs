using System.Threading.Tasks;

namespace Parser.CSAnalizator
{
    public interface IAnalyzer
    {
        public Task<string> AnalizeCategoryAsync(string text, string defaultCategory);
    }
}