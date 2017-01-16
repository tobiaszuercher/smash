    namespace Smash.Model
    {
        public class PlayerRanking
        {
            public long Id { get; set; }
            public string FullName { get; set; }
            public int Points { get; set; }
            public int InterclubPoints { get; set; }
            public int TotalPoints { get; set; }
            public int TournamentCount { get; set; }
            public string Level { get; set; }
        }
    }
