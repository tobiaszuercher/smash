    namespace Smash.Model
    {
        public class WeekEntry
        {
            public string Id { get; set; }
            public int WeekNumber { get; set; }

            public override string ToString()
            {
                return $"{WeekNumber} ({Id})";
            }
        }
    }
