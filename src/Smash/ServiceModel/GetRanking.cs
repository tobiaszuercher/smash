using ServiceStack;
using Smash.Model;

namespace Smash.ServiceModel
{
    [Route("/rankings", "GET")]
    [Route("/rankings/current", "GET")]
    [Route("/rankings/{Week}", "GET")]
    public class GetRanking : IReturn<RankingSnapshot>
    {
        public int Week { get; set; }
    }
}