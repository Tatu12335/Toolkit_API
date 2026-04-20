using AvToolKitWPF.Main;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AvToolKitWPF.Login_Create
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = new HttpClient())
                {
                    var userdata = new
                    {
                        username = UsernameTextbox.Text,
                        password = PasswordBox.Password
                    };

                    var json = JsonConvert.SerializeObject(userdata);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await conn.PostAsync("https://localhost:7023/Login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var Role = response.Headers.GetValues("role").FirstOrDefault();
                        var token = await response.Content.ReadAsStringAsync();
                        
                        MessageBox.Show($"Login Successful", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        var mainWindow = new MainWindow(token,Role);
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Login failed: Check your username and password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            var createAccountWindow = new Create();
            createAccountWindow.Show();
            this.Close();

        }
    }
}
