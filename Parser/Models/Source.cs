using Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Source
    {
       
       
        public Source()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public SourceType SourceType { get; set; }
        public List<Event> Events { get; set; }
        public SourceFields Fields { get; set; }

        public override string ToString()
        {
            return $"Source {Id} - {SourceType}";
        }
    }
}
