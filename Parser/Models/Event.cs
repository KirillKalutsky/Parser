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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Event()
        {
            DateOfDownload = DateTime.Now;
            Hash = GetHashCode();
        }
        public Source Source { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }
        public DateTime DateOfDownload { get; }
        public string Address { get; set; }
        public string Link { get; set; }
        public int Hash { get; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return $"{Id}\n{Link}\n{Body}";
        }
    }
}
