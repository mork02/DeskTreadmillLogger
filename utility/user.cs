using DeskTreadmillLogger.models;

namespace DeskTreadmillLogger.utility
{
    internal class User
    {
        private static string userDataPath = "config/userData.json";
        public static UserEntry currentUser { get; private set; }
        private static List<UserEntry> usersList = new();

        public static void Init()
        {
            usersList = JSONReader.Load<UserEntry>(userDataPath);
        }

        public static void CreateUser(string name, double weightKg, int heightCm)
        {
            int nextId = usersList.Any() ? usersList.Max(u => u.id) + 1 : 1;

            currentUser = new UserEntry
            {
                id = nextId,
                name = name,
                weightKg = weightKg,
                heightCm = heightCm
            };

            usersList.Add(currentUser);
            JSONReader.Add(currentUser, userDataPath);
        }

        public static void DeleteUser(int id)
        {
            JSONReader.Remove<UserEntry>(u => u.id == id, userDataPath);
            usersList.RemoveAll(entry => entry.id == id);
        }

        public static void SelectUser(int id)
        {
            UserEntry temp = usersList.FirstOrDefault(u => u.id == id);
            if (temp == null)
            {
                currentUser = null;
                return;
            }
            currentUser = temp;
        }
    }
}
