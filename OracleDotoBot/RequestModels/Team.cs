using OracleDotoBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OracleDotoBot.RequestModels
{
    public class Team
    {
        public TeamType TeamType { get; set; }

        public int Pos1HeroId { get; set; }

        public int Pos2HeroId { get; set; }

        public int Pos3HeroId { get; set; }

        public int Pos4HeroId { get; set; }

        public int Pos5HeroId { get; set; }
    }
}
