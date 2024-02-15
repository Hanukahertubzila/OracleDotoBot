using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDotoBot.StratzApiParser.OutputDataTypes
{
    public class HeroStatistics
    {
        public int HeroId { get; set; }

        public double WinRate { get; set; }

        public double WinsVs {  get; set; } 

        public double WinsWith { get; set; } 
    }
}
