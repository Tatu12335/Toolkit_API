using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using AvToolKitWPF.Main;

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
                        var token = response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Login Successful", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        var mainWindow = new MainWindow(token.Result);
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Login failed: {response.StatusCode}", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
