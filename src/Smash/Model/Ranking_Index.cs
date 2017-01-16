using System.Linq;
using Raven.Client.Indexes;

namespace Smash.Model
{
    public class Ranking_Index : AbstractIndexCreationTask<RankingSnapshot>
    {
        public class IndexEntry
        {
            public int WeekNumber { get; set; }
            public string WeekId { get; set; }
            public string Season { get; set; }
        }

        public Ranking_Index()
        {
            Map = rankings => from ranking in rankings
                select new IndexEntry()
                {
                    WeekNumber = ranking.Week.WeekNumber,
                    WeekId = ranking.Week.Id,
                    Season = ranking.Season,
                };
        }
    }
}