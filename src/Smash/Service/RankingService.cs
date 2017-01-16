using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;
using ServiceStack;
using Smash.Model;
using Smash.ServiceModel;

namespace Smash.Service
{
    public class RankingService : ServiceStack.Service
    {
        private readonly IDocumentStore _store;

        public RankingService(IDocumentStore store)
        {
            _store = store;
        }

        public List<RankingDto> Get(GetRanking request)
        {
            int weekNo = 0;

            if (request.Week == default(int))
            {
                weekNo = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(DateTime.UtcNow,
                    CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
            else
            {
                weekNo = request.Week;
            }

            var response = new List<RankingDto>();

            using (var session = _store.OpenSession())
            {
                var ranking = session.Query<Ranking_Index.IndexEntry, Ranking_Index>()
                    .Where(r => r.WeekNumber == weekNo)
                    .OfType<RankingSnapshot>()
                    .FirstOrDefault();

                if (ranking == null)
                {
                    throw HttpError.NotFound($"No ranking found for {weekNo}");
                }

                foreach (var playerRanking in ranking.PlayerRankings)
                {
                    response.Add(new RankingDto()
                    {
                        WeekNumber = weekNo,
                        FullName = playerRanking.FullName,
                        InterclubPoints = playerRanking.InterclubPoints,
                        Level = playerRanking.Level,
                        PlayerId = playerRanking.Id,
                        Points = playerRanking.Points,
                    });
                }

                return response;
            }
        }
    }
}