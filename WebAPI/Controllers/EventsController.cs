using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser;
using DB;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : Controller
    {
        private MyDBContext db;
        public EventsController(MyDBContext db)
        {
            this.db = db;
        }
        // GET: Events

        [HttpGet("All")]
        public async Task<string> GetEvents()
        {
            var allEvents = await db.GetAllEventsAsync();
            return JsonConvert.SerializeObject(allEvents);
        }

        [HttpGet("ByTime")]
        public async Task<string> GetEventsByTime(int timeInMinutes)
        {
            var minDate = DateTime.Now.AddMinutes(-1 * timeInMinutes);
            var events = await db.GetAllEventsAsync();
            
            var eventsByTime = events.Where(e => e.DateOfDownload >= minDate);
            Debug.Print($"Send Events by last :{timeInMinutes} minutes \\n date: {minDate} \\n {eventsByTime.Count()} of {events.Count()}");
            return JsonConvert.SerializeObject(eventsByTime);
        }

    }
}
