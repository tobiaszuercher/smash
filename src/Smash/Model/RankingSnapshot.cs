using System.Collections.Generic;

namespace Smash.Model
{
    public class RankingSnapshot
    {
        public string Season { get; set; }
        public WeekEntry Week { get; set; }
        public List<PlayerRanking> PlayerRankings { get; set; }

        public RankingSnapshot()
        {
            PlayerRankings = new List<PlayerRanking>();
        }
    }
}