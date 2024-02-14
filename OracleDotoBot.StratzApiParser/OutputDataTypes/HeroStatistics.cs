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

        public List<double> WinsVs {  get; set; } = new List<double>();

        public List<double> WinsWith { get; set; } = new List<double>();
    }
}
