using System.Windows;

namespace AvToolKitWPF.AdminPanel
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private readonly string _token;
        public AdminPanel(string token)
        {
            InitializeComponent();
            _token = token;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonManageUsers_Click(object sender, RoutedEventArgs e)
        {
            ManageUsersWindow manageUsersWindow = new ManageUsersWindow(_token);
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
