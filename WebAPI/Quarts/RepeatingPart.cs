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
        Dictionary<string, HashSet<string>> categories = new Dictionary<string, HashSet<string>>()
            {
                {"ДТП" , new HashSet<string>() 
                {
                    "водитель", "пассажир", "протаранить", "пассажирка", "пешеход", "автомобиль", "иномарка",
                    "мотоцикл", "грузовик", "пролететь", "проехать", "наехать", "сбить", "выехать", "вылететь", 
                    "столкнуться", "врезаться", "авария" , "автомобиль", "промчаться", "двигаться", "обогнать", 
                    "обгон"
                }},
                {"Пожар" , new HashSet<string>() 
                {
                    "сгореть", "загореться" , "огонь", "пламя", "потушить", "воспламяниться", "гореть", 
                    "пожар", "вспыхнуть", "взорваться", "возгорание", "задымление", "дым", "газ"
                }},
                {"Физическое насилие" , new HashSet<string>()
                {
                    "убийца", "убитый", "убитая", "убитые", "труп", "тело", "убийство", "вооружиться", "вооружился",
                    "вооружилась", "нож", "ранение", "раны", "рана", "зарезать", "скончаться", "удар", "ударить",
                    "конфликт", "ссора", "поссориться", "нападавший", "жестокость", "нападавшая","нападать", 
                    "огнестрельное", "оружие", "выстрел", "стрельба", "задушить", "удушье", "удушение", "изрубить",
                    "кровь", "кровотечение", "мучить", "пытки", "пытать", "смертельный", "травмы", "сжёг", "насильственная",
                    "расправиться", "развратные", "изнасилование", "изнасиловать", "сексуальное",  "насилие", "драка", 
                    "рукоприкладство", "избиение", "жертва", "избивать", "бить", "пинать", "побои"
                }},
                {"Кражи" , new HashSet<string>()
                {
                    "украсть", "ограбить", "кража", "грабитель", "вор", "хищение", "ограбление", "обокрасть", "похитить",
                    "взлом", "ценности"
                }},
                {"Суицид" , new HashSet<string>() 
                {
                    "покончить", "суицид", "падение", "упасть", "самоубийство", "сброситься", "повеситься", "застрелиться" 
                }},
            };
        public RepeatingPart(IHttpClientFactory clientFactory/* IDBContext dbContext*/) 
        {
            analizator = new Analyzer(categories, new MorphAnalyzer(withLemmatization: true, withTrimAndLower: true));
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
                Debug.Print("Body: "+e.Body);

                var category = await analizator.AnalizeCategoryAsync(e.Body, defaultCategory);
                e.IncidentCategory = category;

                if (category != defaultCategory)
                {
                    var distr = await districtAnalyzer.AnalyzeDistrict(e.Body);
                    e.District = distr;
                    Debug.Print(e.District.DistrictName);
                }

                counter++;
                await dbContext.AddEventAsync(e);

                Debug.Print(e.Link);
                
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
