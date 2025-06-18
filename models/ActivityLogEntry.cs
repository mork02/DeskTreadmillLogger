namespace DeskTreadmillLogger.models
{
    internal class ActivityLogEntry
    {
        public int id { get; set; }
        public int userId { get; set; }
        public UserEntry user { get; set; }
        public DateTime timestamp { get; set; } = DateTime.UtcNow;
        public double distanceKm { get; set; }  
        public TimeSpan duration { get; set; }  
        public double caloriesBurned { get; set; }
        public string notes { get; set; }
    }
}
