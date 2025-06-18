using DeskTreadmillLogger.models;

namespace DeskTreadmillLogger.utility
{
    internal class ActivityLog
    {
        private static string activityDataPath = "config/activityLog.json";
        private static List<ActivityLogEntry> activityList = new();
        private static readonly List<(double maxSpeed, double met)> metMap = new()
        {
            (2, 2.0),
            (3, 2.8),
            (4, 3.3),
            (5, 3.8),
            (6, 4.5),
            (7, 6.0),
            (8, 7.0),
            (9, 8.3),
            (10, 9.0)
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

        private static double GetMetFromSpeed(double speedKmH)
        {
            foreach (var (maxSpeed, met) in metMap)
            {
                if (speedKmH < maxSpeed)
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
