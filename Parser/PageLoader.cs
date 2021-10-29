using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class PageLoader
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<Tuple<HttpResponseMessage,string>> LoadPage(string url, int numberOfAttempts = 5)
        {
            HttpResponseMessage result;
            try
            {
                result = await httpClient.GetAsync(url);
            }
            catch
            {
                Console.WriteLine("Ошибка при загрузке");
                Debug.Print("Ошибка при загрузке");
                result = new HttpResponseMessage();
                result.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            if (!result.IsSuccessStatusCode && numberOfAttempts > 0)
            {
                Console.WriteLine($"BadStatusCode {numberOfAttempts}");
                Console.WriteLine($"{url}");
                await Task.Delay(500);
                result = (await LoadPage(url, numberOfAttempts - 1)).Item1;
            }
            Console.WriteLine($"AllGood {numberOfAttempts}");
            return Tuple.Create(result,url);
        }
    }
}
