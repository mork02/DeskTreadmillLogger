using DeskTreadmillLogger.utility;
using DeskTreadmillLogger.models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DeskTreadmillLogger
{
    public partial class ActivityViewControl : UserControl
    {

        public ActivityViewControl()
        {
            InitializeComponent();
            LoadActivities();
        }

        private void LoadActivities()
        {
            UserLabel.Text = User.currentUser?.name;
            ActivityList.Items.Clear();
            var Activitylist = JSONReader.Load<ActivityLogEntry>("config/activityLog.json")
                        .Where(a => a.user.id == User.currentUser.id)
                        .OrderBy(a => a.timestamp);

            foreach (var activity in Activitylist)
                ActivityList.Items.Add($"Activity: {activity.id}");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(SpeedBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double speed) ||
                !TimeSpan.TryParse(DurationBox.Text, out TimeSpan duration))
            {
                MessageBox.Show("Invalid inputs for speed or duration.");
                return;
            }

            string notes = NotesBox.Text;
            try
            {
                ActivityLog.CreateActivity(speed, duration, notes);
                MessageBox.Show("Activity saved.");
                LoadActivities();
                SpeedBox.Clear();
                DurationBox.Clear();
                NotesBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message);
            }
        }
    }
}
