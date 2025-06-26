using System.Windows;

namespace DeskTreadmillLogger
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new UserSelectionControl(this); // zeigt zuerst Benutzerwahl
        }

        public void NavigateToActivityView()
        {
            MainContent.Content = new ActivityViewControl(); // wechselt zur Aktivitätsübersicht
        }
    }
}
