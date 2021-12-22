using Newtonsoft.Json;
using Parser.Infrastructure.Python;
using Parser.Python;
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
        private PythonExecutor pythonAnalyzer;
        public DistrictAnalyzer(IEnumerable<District> districts, IEnumerable<Address> addresses)
        {
            this.districts = districts.ToHashSet();
            this.addresses = addresses.ToHashSet();
            pythonAnalyzer = new PythonExecutor(@"D:\anaconda\python.exe", @"D:\anaconda\Natasha\newsAnalysis\myScripts\1.py");
        }

        public District AnalyzeDistrict(string text)
        {
            var res = pythonAnalyzer.ExecuteScript(text);
            var output = JsonConvert.DeserializeObject<ScriptResponse>(res);
            foreach (var name in output.Names) 
            {
                var distr = districts.Where(distr => distr.DistrictName == name.ToLower()).FirstOrDefault();
                if (distr != null)
                    return distr;
            }
            var address = addresses.Where(adr => adr.AddressName.ToLower().Contains(output.Addresses.FirstOrDefault().Value)).FirstOrDefault();

            if (address != null)
                return address.District;

            return districts.Where(d => d.DistrictName == "None").FirstOrDefault();
        }
    }
}
