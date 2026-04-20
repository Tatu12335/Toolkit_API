using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace AvToolKitWPF.Login_Create
{
    /// <summary>
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password) || string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password) || string.IsNullOrEmpty(EmailTextBox.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }
                if (PasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                using (var client = new HttpClient())
                {
                    var userData = new
                    {
                        username = UsernameTextBox.Text,
                        email = EmailTextBox.Text,
                        password = PasswordBox.Password,
                        role = "User"
                    };

                    var json = JsonConvert.SerializeObject(userData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://localhost:7023/Register", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Failed to create account. Please try again.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return;
            }

            MessageBox.Show("Account created successfully! Please log in.");
            this.Close();
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {


        }
    }
}
