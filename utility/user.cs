using DeskTreadmillLogger.models;
using System.IO;
using System.Windows.Navigation;

namespace DeskTreadmillLogger.utility
{
    internal class User
    {
        private static string userDataPath = "config/userData.json";
        public static UserEntry currentUser;
        private static List<UserEntry> usersList = new();

        public static void Init()
        {
            if (!File.Exists(userDataPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(userDataPath));
                File.WriteAllText(userDataPath, "[]");
            }

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
            JSONReader.Save(usersList, userDataPath);
        }

        public static void DeleteUser(int id)
        {
            SelectUser(id);
            usersList.Remove(currentUser);
            JSONReader.Remove(currentUser, userDataPath);
        }

        public static void SelectUser(int id)
        {
            var temp = usersList.FirstOrDefault(u => u.id == id);
            if (temp == null)
            {
                return;
            }
            else
            {
                currentUser = temp;
            }
        }

        public static List<UserEntry> GetAllUsers()
        {
            return usersList;
        }

    }
}
