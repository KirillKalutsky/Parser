using DB;
using DeepMorphy;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Parser;
using Parser.CSAnalizator;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPI.Quarts
{
    public class RepeatingPart : IJob
    {
        private readonly DistrictAnalyzer districtAnalyzer;
        IHttpClientFactory clientFactory;
        MyDBContext dbContext;
        Crawler crawler;
        Analyzer analizator;
        private readonly string defaultCategory = "Не ЧП";
        
        public RepeatingPart(IHttpClientFactory clientFactory/* IDBContext dbContext*/) 
        {
            analizator = new Analyzer(Emergency.Categories, defaultCategory);
            this.clientFactory = clientFactory;
            crawler = new Crawler();
            this.dbContext = new MyDBContext();
            districtAnalyzer = new DistrictAnalyzer(dbContext.Districts, dbContext.Addresses.Include(adr=>adr.District));

        }

        public async Task Execute(IJobExecutionContext context)
        {

            Debug.Print("Quarts");
            var startTime = DateTime.Now;
            var counter = 1;

            var sources = await dbContext.GetSourcesAsync();

            Debug.Print("Источники:");
            foreach (var s in sources)
            {
                Debug.Print(s.Fields.Properties);

                foreach (var e in s.Events)
                    Debug.Print(e.Link);
            }

            await foreach (var e in crawler.StartAsync(sources))
            {
                Debug.Print(counter.ToString());
                Debug.Print("Body: " + e.Body);

                var category = await analizator.AnalizeCategoryAsync(e.Body);
                e.IncidentCategory = category;

                if (category != defaultCategory)
                {
                    var distr = await districtAnalyzer.AnalyzeDistrict(e.Body);
                    e.District = distr;
                    Debug.Print(e.District.DistrictName);
                }

                counter++;
                await dbContext.AddEventAsync(e);

                if(counter%100 == 0)
                    dbContext.SaveChanges();
                Debug.Print(e.Link);
                Debug.Print(e.IncidentCategory);
                Debug.Print("\n");
            }

            Debug.Print("Start Save Changes");
            dbContext.SaveChanges();
            Debug.Print("Finish Save Changes");

            Console.Write(DateTime.Now - startTime);
            Debug.Print((DateTime.Now - startTime).ToString());
        }
    }
}
