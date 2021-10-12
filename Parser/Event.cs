using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Event
    {
        public string Body { get; set; }

        private string date;
        public string Date 
        {
            get
            {
                return date;
            }
            set
            {
                if (value.ToLower().Contains("сегодня"))
                    date = DateTime.Now.ToString();
                else
                    date = value;
            }
        }
        public string Address { get; set; }
        public string Link { get; set; }
        public int Hash { get; set; }
    }
}
