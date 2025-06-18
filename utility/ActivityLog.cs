using DeskTreadmillLogger.models;

namespace DeskTreadmillLogger.utility
{
    internal class ActivityLog
    {
        private static string activityDataPath = "config/activityLog.json";
        private static List<ActivityLogEntry> activityList = new();
        private static readonly List<(double maxSpeed, double met)> metMap = new()
        {
            (2.0, 2.0),     // Very slow walk
            (2.7, 2.3),     // Slow walk
            (3.2, 2.5),     // Gentle stroll
            (3.7, 2.33),    // Easy pace
            (4.0, 2.8),     // Normal walk
            (4.5, 3.0),     // Fast walk
            (5.0, 3.3),     // Brisk walk
            (5.5, 3.6),     // Very brisk walk
            (6.0, 4.0),     // Power walk / slow jog
            (6.5, 4.5),     // Light jog
            (7.0, 5.0),     // Jogging
            (7.5, 6.0),     // Jogging ~7.5 km/h
            (8.0, 7.0),     // Jogging ~8 km/h
            (9.0, 8.0),     // Running ~9 km/h
            (10.0, 9.0),    // Running ~10 km/h
        };

        public static void Init()
        {
            activityList = JSONReader.Load<ActivityLogEntry>(activityDataPath);
        }

        public static void CreateActivity(double speed, TimeSpan duration, string? notes = "")
        {
            if (User.currentUser == null)
                throw new InvalidOperationException("Current user is not set.");

            int nextId = activityList.Any() ? activityList.Max(u => u.id) + 1 : 1;

            ActivityLogEntry entry = new ActivityLogEntry
            {
                id = nextId,
                user = User.currentUser,
                speed = speed,
                duration = duration,
                notes = notes
            };

            entry.distanceKm = CalculateDistanceKm(entry);
            entry.caloriesBurned = (int)CalculateBurnedKcal(entry);

            activityList.Add(entry);
            JSONReader.Add(entry, activityDataPath);
        }

        private static double CalculateDistanceKm(ActivityLogEntry entry)
        {
            return entry.speed * entry.duration.TotalHours;
        }

        private static double CalculateBurnedKcal(ActivityLogEntry entry)
        {
            if (User.currentUser == null)
                throw new InvalidOperationException("Current user is not set.");

            double met = GetMetFromSpeed(entry.speed);
            double kcalPerMinute = (met * User.currentUser.weightKg * 3.5) / 200.0;
            return kcalPerMinute * entry.duration.TotalMinutes;
        }

        private static double GetMetFromSpeed(double speed)
        {
            foreach (var (maxSpeed, met) in metMap)
            {
                if (speed < maxSpeed)
                    return met;
            }

            return metMap.Last().met;
        }

        public static void DeleteActivity(int id)
        {
            JSONReader.Remove<UserEntry>(u => u.id == id, activityDataPath);
            activityList.RemoveAll(entry => entry.id == id);
        }

    }
}
