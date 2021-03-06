/*using DataSources.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    public class VKAPI
    {
        readonly HttpClient httpClient;

        VKGroup vk;
        public VKAPI(HttpClient httpClient,VKGroup vk)
        {
            this.httpClient = httpClient;
            this.vk = vk;
        }

        

        public Task<List<Publication>> DownloadAsync(DataSource source)
        {
            return ScripeVKPosts(vk.SourceGroup,source);
        }

        public  Task<string> DoMethod(string method, string par, string v = "5.131")
        {
            var req = $"https://api.vk.com/method/{method}?{par}&access_token={vk.token}&v={v}";

            return httpClient.GetStringAsync(req);
        }

        private  async Task<List<Publication>> ScripeVKPosts(List<string> groups, DataSource source)
        {
            var flag = true;
            var publications = new List<Publication>();
            foreach (var g in groups)
            {
                var count = 0;
                var curGroupPost = new List<Task<string>>();
                while (flag)
                {
                    var posts = DoMethod("wall.get", $"domain={g}&count=100&offset={count}");
                    curGroupPost.Add(posts);
                    count += 100;

                    while (curGroupPost.Any())
                    {
                        var ps = await Task.WhenAny(curGroupPost);
                        curGroupPost.Remove(ps);

                        var res = PrettyJson(await ps);
                        foreach (var line in GetMaintDataOfPost(g, res,source))
                        {
                            publications.Add(line);

                            if (line.Date<DateTime.Now.AddDays(-2))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                }
                flag = true;
            }
            return publications; 
        }

        public  string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(unPrettyJson);
            return System.Text.Json.JsonSerializer.Serialize(jsonElement, options);
        }

        private  List<Publication> GetMaintDataOfPost(string sourceName,string json, DataSource source)
        {
            //классы Response и Post нужны для удобной работы с JSON
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, Response>>(json);
            if (parsed == null) return null;
            var result = new List<Publication>();
            //там только один ключ - "response"
            foreach (var item in parsed["response"].items)
            {
               
                if (!(String.IsNullOrEmpty(item.text) || String.IsNullOrWhiteSpace(item.text) || String.Empty.CompareTo(item.text) == 0))
                    result.Add(new Publication {Text = item.text, Date = GetTime(item.date), URL = $"https://vk.com/{sourceName}?w=wall{item.owner_id}_{item.id}",Source = source });
                //builder.AppendLine(UnixTimeToDateTime(long.Parse(item.date)).ToString());
            }
            return result;
        }

        public  DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        public  DateTime GetTime(string time)
        {
            DateTime date;
            try
            {
                var longTime = long.Parse(time);
                date = UnixTimeToDateTime(longTime);
            }
            catch (FormatException e)
            {
                Debug.Print(e.Message);
                date = DateTime.Now;
            }
            return date;
        }
    }
}
*/