using DeskTreadmillLogger.models;
using DeskTreadmillLogger.utility;
using System.Windows;
using System.Windows.Controls;

namespace DeskTreadmillLogger
{
    public partial class UserSelectionControl : UserControl
    {
        private MainWindow _main;

        public UserSelectionControl(MainWindow main)
        {
            InitializeComponent();
            _main = main;

            User.Init();
            UserComboBox.ItemsSource = JSONReader.Load<UserEntry>("config/userData.json");
        }

        private void SelectUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedItem is UserEntry user)
            {
                User.SelectUser(user.id);
                _main.NavigateToActivityView();
            }
            else
            {
                MessageBox.Show("Please select a user.");
            }
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            bool weightOk = double.TryParse(WeightBox.Text, out double weight);
            bool heightOk = int.TryParse(HeightBox.Text, out int height);
            DateTime? birthdate = BirthDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(name) || !weightOk || !heightOk || birthdate == null)
            {
                MessageBox.Show("Please fill in all mandatory fields correctly.");
                return;
            }

            var newUser = new UserEntry
            {
                name = name,
                weightKg = weight,
                heightCm = height,
                birthdate = birthdate.Value,
                email = EmailBox.Text,
                gender = "Unknown"
            };

            User.CreateUser(newUser.name, newUser.weightKg, newUser.heightCm);
            var list = JSONReader.Load<UserEntry>("config/userData.json");
            var created = list.LastOrDefault();
            if (created != null)
            {
                created.birthdate = newUser.birthdate;
                created.email = newUser.email;
                JSONReader.Remove<UserEntry>(u => u.id == created.id, "config/userData.json");
                JSONReader.Add(created, "config/userData.json");
                User.SelectUser(created.id);
                _main.NavigateToActivityView();
            }
        }
    }
}
