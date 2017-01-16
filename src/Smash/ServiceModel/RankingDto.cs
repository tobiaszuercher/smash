namespace Smash.ServiceModel
{
    public class RankingDto
    {
        public long PlayerId { get; set; }
        public string FullName { get; set; }
        public int Points { get; set; }
        public int InterclubPoints { get; set; }
        public int TotalPoints { get; set; }
        public int TournamentCount { get; set; }
        public string Level { get; set; }
        public int WeekNumber { get; set; }
    }
}