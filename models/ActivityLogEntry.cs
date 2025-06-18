namespace DeskTreadmillLogger.models
{
    internal class ActivityLogEntry
    {
        public int id { get; set; }
        public UserEntry user { get; set; }
        public DateTime timestamp { get; set; } = DateTime.UtcNow;
        public double speed { get; set; }
        public TimeSpan duration { get; set; }  
        public double distanceKm { get; set; }  
        public int caloriesBurned { get; set; }
        public string notes { get; set; }
    }
}
