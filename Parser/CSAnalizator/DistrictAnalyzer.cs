using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.CSAnalizator
{
    public class DistrictAnalyzer
    {
        private HashSet<District> districts;
        private HashSet<Address> addresses;
        public DistrictAnalyzer(IEnumerable<District> districts, IEnumerable<Address> addresses)
        {
            this.districts = districts.ToHashSet();
            this.addresses = addresses.ToHashSet();
        }

        public District AnalyzeDistrict(string text)
        {
            throw new NotImplementedException();
        }
    }
}
