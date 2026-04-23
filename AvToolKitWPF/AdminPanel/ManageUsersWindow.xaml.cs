using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using Toolkit_API.Domain.Entities.Users;

namespace AvToolKitWPF.AdminPanel
{
    /// <summary>
    /// Interaction logic for ManageUsersWindow.xaml
    /// </summary>
    public partial class ManageUsersWindow : Window
    {
        private readonly string _token;
        public ManageUsersWindow(string token)
        {
            _token = token;
            InitializeComponent();
            this.Loaded += async (s, e) =>
            {
                try
                {
                    using (var conn = new HttpClient())
                    {
                        conn.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                        var response = await conn.GetAsync("https://localhost:7023/Admin/GetAllUsers");
                        if (response.IsSuccessStatusCode)
                        {
                            var users = await response.Content.ReadFromJsonAsync<List<string>>();
                            UsersListBox.Items.Clear();
                            if (users == null || users.Count == 0)
                            {
                                MessageBox.Show("No users found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                            foreach (var user in users)
                            {
                                UsersListBox.Items.Add(user);
                            }

                        }
                        else
                        {
                            MessageBox.Show($"Failed to load users: {response.ReasonPhrase}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($" An error occured while loading users : {ex.Message}", " Error", MessageBoxButton.OK);
                    return;
                }
            };
        }
        public async Task<List<string>> GetAllUsers()
        {
            try
            {
                using (var conn = new HttpClient())
                {
                    conn.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var response = await conn.GetAsync("https://localhost:7023/Admin/GetAllUsers");
                    if (response.IsSuccessStatusCode)
                    {
                        var users = await response.Content.ReadFromJsonAsync<List<string>>();
                        return users ?? new List<string>();
                    }
                    else
                    {
                        MessageBox.Show($"Failed to load users: {response.ReasonPhrase}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return new List<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" An error occured while loading users : {ex.Message}", " Error", MessageBoxButton.OK);
                return new List<string>();
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = new HttpClient())
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(SearchUsernameTextBox.Text);

                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    conn.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                    var response = await conn.PostAsync("https://localhost:7023/Admin/SearchByUsername", content);

                    if (response.IsSuccessStatusCode)
                    {

                        var user = await response.Content.ReadFromJsonAsync<ForAdminEntity>();
                        if (user != null)
                        {
                            UsersListBox.Items.Clear();
                            UsersListBox.Items.Add($"ID: {user.id} | Username: {user.username} | Email: {user.newemail} | Roles: {user.roles}");
                        }

                    }

                    UsersListBox.Items.Clear();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" An error occured while performing search : {ex.Message}", " Error", MessageBoxButton.OK);
                return;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var users = await GetAllUsers();

                if (users == null || users.Count == 0)
                    return;
                foreach (var user in users)
                {
                    UsersListBox.Items.Clear();
                    UsersListBox.Items.Add(user);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($" An error occured while loading users : {ex.Message}", " Error", MessageBoxButton.OK);
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private async void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {



        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void UsersListBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
