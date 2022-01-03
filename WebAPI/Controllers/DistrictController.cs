using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistrictController : Controller
    {
        private MyDBContext db;
        public DistrictController(MyDBContext db)
        {
            this.db = db;
        }

        [HttpGet("ByName")]
        public async Task<string> GetDistrictByName(string districtName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            var district = await db.GetDistrictByNameAsync(districtName);

            return JsonConvert.SerializeObject(district, settings);
        }

        [HttpGet("Events")]
        public async Task<string> GetDistrictEvents(string districtName, DateTime? lastEventDownloadTime)
        {
            Debug.Print("DoubleGet "+districtName+" "+ lastEventDownloadTime.ToString());
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var events = (await db.GetDistrictByNameAsync(districtName)).Events;

            if(lastEventDownloadTime == null)
                return JsonConvert.SerializeObject(events, settings);

            var newEvents = events.Where(ev => ev.DateOfDownload > lastEventDownloadTime);

            return JsonConvert.SerializeObject(newEvents, settings);
        }

    }
}
