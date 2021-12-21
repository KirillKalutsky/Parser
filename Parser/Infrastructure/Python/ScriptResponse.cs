using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Infrastructure.Python
{
    public class ScriptResponse
    {
        public List<string> Names { get; set; }
        public List<AddrPart> Addresses { get; set; }
    }
}
