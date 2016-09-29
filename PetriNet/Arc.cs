using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUS
{
    public class Arc
    {
        public string Type { get; set; }
        public int SourceID { get; set; }
        public int DestinationID { get; set; }
        public int Multiplicity { get; set; }
    }
}
