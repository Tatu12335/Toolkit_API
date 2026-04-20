using System.Windows;

namespace AvToolKitWPF.AdminPanel
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonManageUsers_Click(object sender, RoutedEventArgs e)
        {
            ManageUsersWindow manageUsersWindow = new ManageUsersWindow();
            manageUsersWindow.ShowDialog();
        }
        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonViewLogs_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
