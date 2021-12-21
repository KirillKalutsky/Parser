using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Event
    {
        public Event()
        {

        }
        [Key]
        public string Link { get; set; }
        public Source Source { get; set; }
        public string Title { get; set; }
        public string IncidentCategory {get;set;}
        public string Body { get; set; }
        public string Date { get; set; }
        public DateTime DateOfDownload { get; set; }
        public Address Address { get; set; }

        [Column(TypeName = "jsonb")]
        public HashSet<string> Hash { get; }

        public override int GetHashCode()
        {
            return Link.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Event))
                return false;
            
            return ((Event)obj).Link.Equals(Link);
        }

        public override string ToString()
        {
            return $"{Link}\n{Body}";
        }
    }
}
