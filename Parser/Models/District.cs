using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class District
    {
        public District()
        {
            Addresses = new List<Address>();
        }

        public District(string name)
        {
            DistrictName  = name;
            Addresses = new List<Address>();
        }

        [Key]
        public string DistrictName  { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
